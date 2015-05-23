using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using ImageEditing.Exceptions;
using ImageEditing.HelperClasses;

namespace ImageEditing.BaseClasses
{
	public class UnsafeImage : IDisposable
	{
		#region Private members

		private IntPtr imageData;
		private readonly int width;
		private readonly int height;
		private readonly int stride;
		private readonly PixelFormat pixelFormat;
		private bool mustBeDisposed = false;

		#endregion

		#region Public properties

		public IntPtr ImageData
		{
			get { return imageData; }
		}

		public int Width
		{
			get { return width; }
		}

		public int Height
		{
			get { return height; }
		}

		public int Stride
		{
			get { return stride; }
		}

		public PixelFormat PixelFormat
		{
			get { return pixelFormat; }
		}

		#endregion

		#region Constructors/destructors

		public UnsafeImage(IntPtr imageData, int width, int height, int stride, PixelFormat pixelFormat)
		{
			this.imageData = imageData;
			this.width = width;
			this.height = height;
			this.stride = stride;
			this.pixelFormat = pixelFormat;
		}

		public UnsafeImage(BitmapData bitmapData) :
			this(bitmapData.Scan0, bitmapData.Width, bitmapData.Height, bitmapData.Stride, bitmapData.PixelFormat)
		{ }

		~UnsafeImage()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{ }

			if (mustBeDisposed && imageData != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(imageData);
				imageData = IntPtr.Zero;
			}
		}

		#endregion

		#region Copy/clone unmanaged images

		public UnsafeImage Clone()
		{
			// allocate memory for the image
			IntPtr newImageData = Marshal.AllocHGlobal(stride * height);

			UnsafeImage newImage = new UnsafeImage(newImageData, width, height, stride, pixelFormat);
			newImage.mustBeDisposed = true;

			MemoryTools.CopyUnmanagedMemory(newImageData, imageData, stride * height);

			return newImage;
		}

		public void Copy(UnsafeImage dstImage)
		{
			if (
				width != dstImage.width ||
				height != dstImage.height ||
				pixelFormat != dstImage.pixelFormat)
			{
				throw new InvalidImageException("Destination image has different size or pixel format.");
			}

			if (stride == dstImage.stride)
			{
				// copy entire image
				MemoryTools.CopyUnmanagedMemory(dstImage.imageData, imageData, stride * height);
			}
			else
			{
				unsafe
				{
					int dstStride = dstImage.stride;
					int copyLength = (stride < dstStride) ? stride : dstStride;

					byte* src = (byte*)imageData.ToPointer();
					byte* dst = (byte*)dstImage.imageData.ToPointer();

					// copy line by line
					for (int i = 0; i < height; i++)
					{
						MemoryTools.CopyUnmanagedMemory(dst, src, copyLength);

						dst += dstStride;
						src += stride;
					}
				}
			}
		}

		public static UnsafeImage Create(int width, int height, PixelFormat pixelFormat)
		{
			// check image size
			if (width <= 0 || height <= 0)
			{
				throw new InvalidImageException("Invalid image size specified.");
			}

			int bytesPerPixel = pixelFormat.GetBytesPerPixel();

			// calculate stride
			int stride = width * bytesPerPixel;

			if (stride % 4 != 0)
			{
				stride += (4 - (stride % 4));
			}

			// allocate memory for the image
			IntPtr imageData = Marshal.AllocHGlobal(stride * height);
			MemoryTools.SetUnmanagedMemory(imageData, 0, stride * height);

			UnsafeImage image = new UnsafeImage(imageData, width, height, stride, pixelFormat);
			image.mustBeDisposed = true;

			return image;
		}

		#endregion

		#region From/to managed image

		public Bitmap ToManagedImage()
		{
			return this.ToManagedImage(true);
		}

		public Bitmap ToManagedImage(bool makeCopy)
		{
			Bitmap dstImage = null;

			try
			{
				if (!makeCopy)
				{
					dstImage = new Bitmap(width, height, stride, pixelFormat, imageData);
				}
				else
				{
					// create new image of required format
					dstImage = new Bitmap(width, height, pixelFormat);

					// lock destination bitmap data
					BitmapData dstData = dstImage.LockBits(
						new Rectangle(0, 0, width, height),
						ImageLockMode.ReadWrite, pixelFormat);

					int dstStride = dstData.Stride;
					int lineSize = Math.Min(stride, dstStride);

					unsafe
					{
						byte* dst = (byte*)dstData.Scan0.ToPointer();
						byte* src = (byte*)imageData.ToPointer();

						if (stride != dstStride)
						{
							// copy image
							for (int y = 0; y < height; y++)
							{
								MemoryTools.CopyUnmanagedMemory(dst, src, lineSize);
								dst += dstStride;
								src += stride;
							}
						}
						else
						{
							MemoryTools.CopyUnmanagedMemory(dst, src, stride * height);
						}
					}

					// unlock destination images
					dstImage.UnlockBits(dstData);
				}

				return dstImage;
			}
			catch (Exception)
			{
				if (dstImage != null)
				{
					dstImage.Dispose();
				}

				throw new InvalidImageException("The unmanaged image has some invalid properties, which results in failure of converting it to managed image.");
			}
		}

		public static UnsafeImage FromManagedImage(Bitmap image)
		{
			UnsafeImage dstImage = null;

			BitmapData srcData = image.LockBits(
				new Rectangle(0, 0, image.Width, image.Height),
				ImageLockMode.ReadOnly, image.PixelFormat);

			try
			{
				dstImage = FromManagedImage(srcData);
			}
			finally
			{
				image.UnlockBits(srcData);
			}

			return dstImage;
		}

		public static UnsafeImage FromManagedImage(BitmapData imageData)
		{
			PixelFormat pixelFormat = imageData.PixelFormat;

			// check source pixel format
			if (!pixelFormat.IsSupportedPixelFormat())
			{
				throw new InvalidPixelFormatException("Unsupported pixel format of the source image.");
			}

			// allocate memory for the image
			IntPtr dstImageData = Marshal.AllocHGlobal(imageData.Stride * imageData.Height);

			UnsafeImage image =
				new UnsafeImage(dstImageData, imageData.Width, imageData.Height, imageData.Stride, pixelFormat);
			MemoryTools.CopyUnmanagedMemory(dstImageData, imageData.Scan0, imageData.Stride * imageData.Height);
			image.mustBeDisposed = true;

			return image;
		}

		#endregion
	}
}
