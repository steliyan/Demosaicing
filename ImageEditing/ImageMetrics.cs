using System;
using System.Drawing;
using System.Drawing.Imaging;
using ImageEditing.BaseClasses;
using ImageEditing.Exceptions;
using ImageEditing.HelperClasses;

namespace ImageEditing
{
	public class ImageMetrics
	{
		private const int Precision = 4;
		private static readonly byte maxIntensity = byte.MaxValue;
		private static readonly int maxIntensitySquared = maxIntensity * maxIntensity;

		private readonly Image srcImage;
		private readonly Image dstImage;
		private readonly int width;
		private readonly int height;

		private double mse;
		private double psnr;
		private readonly bool areMetricsCalculated = false;

		public int MSE
		{
			get { return (int)mse; }
		}

		public string MSEFriendlyName
		{
			get { return string.Format("MSE: {0}", MSE);  }
		}

		public double PSNR
		{
			get { return Math.Round(psnr, Precision); }
		}

		public string PSNRFriendlyName
		{
			get
			{
				return
					double.IsInfinity(psnr) ?
					"Same images!" :
					string.Format("PSNR: {0}db!", PSNR);
			}
		}

		public ImageMetrics(Image srcImage, Image dstImage)
		{
			if (
				srcImage.Size != dstImage.Size ||
				srcImage.PixelFormat != dstImage.PixelFormat
				)
			{
				throw new InvalidImageException("Images are not compatible - size or pixel format mismatch!");
			}

			this.srcImage = srcImage;
			this.dstImage = dstImage;
			this.width = srcImage.Width;
			this.height = srcImage.Height;
			this.CalculateMetrics();
		}

		private void CalculateMetrics()
		{
			if (!areMetricsCalculated)
			{
				CalculateMetrics(new Bitmap(srcImage), new Bitmap(dstImage));
			}
		}

		private void CalculateMetrics(Bitmap srcBitmap, Bitmap dstBitmap)
		{
			// lock source/destination bitmaps
			BitmapData srcData = srcBitmap.LockBits(
				new Rectangle(0, 0, width, height),
				ImageLockMode.ReadOnly, srcBitmap.PixelFormat);
			BitmapData dstData = dstBitmap.LockBits(
				new Rectangle(0, 0, width, height),
				ImageLockMode.ReadOnly, dstBitmap.PixelFormat);

			try
			{
				CalculateMetrics(new UnsafeImage(srcData), new UnsafeImage(dstData));
			}
			finally
			{
				// unlock source/destination bitmaps
				srcBitmap.UnlockBits(srcData);
				dstBitmap.UnlockBits(dstData);
			}
		}

		private unsafe void CalculateMetrics(UnsafeImage srcData, UnsafeImage dstData)
		{
			// get source/destination strides, pixelsizes, offsets, etc..
			int srcStride = srcData.Stride;
			int dstStride = dstData.Stride;

			int srcPixelSize = PixelFormatHelpers.GetBytesPerPixel(srcData.PixelFormat);
			int dstPixelSize = PixelFormatHelpers.GetBytesPerPixel(dstData.PixelFormat);

			int srcOffset = srcStride - width * srcPixelSize;
			int dstOffset = dstStride - width * dstPixelSize;

			// get source/destination pointers
			byte* src = (byte*)srcData.ImageData.ToPointer();
			byte* dst = (byte*)dstData.ImageData.ToPointer();

			// mean squared error for each color component
			ulong mseR = 0;
			ulong mseG = 0;
			ulong mseB = 0;

			// for each line
			for (int y = 0; y < height; y++)
			{
				// for each pixel
				for (int x = 0; x < width; x++, src += srcPixelSize, dst += dstPixelSize)
				{
					int rDiff = src[RGB.R] - dst[RGB.R];
					int gDiff = src[RGB.G] - dst[RGB.G];
					int bDiff = src[RGB.B] - dst[RGB.B];

					if (rDiff != 0)
					{
						mseR += (ulong)(rDiff * rDiff);
					}

					if (gDiff != 0)
					{
						mseG += (ulong)(gDiff * gDiff);
					}

					if (bDiff != 0)
					{
						mseB += (ulong)(bDiff * bDiff);
					}
				}

				src += srcOffset;
				dst += dstOffset;
			}

			mse = (mseR + mseG + mseB) / (3.0 * width * height);
			psnr = 10 * Math.Log10(maxIntensitySquared / mse);
		}
	}
}
