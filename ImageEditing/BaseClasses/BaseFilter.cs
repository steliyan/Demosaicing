using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using ImageEditing.Exceptions;
using ImageEditing.HelperClasses;
using ImageEditing.BaseInterfaces;

namespace ImageEditing.BaseClasses
{
	public abstract class BaseFilter : IFilter, IFilterInformation
	{
		public abstract Dictionary<PixelFormat, PixelFormat> FormatTranslations { get; }

		public Bitmap Apply(Image image)
		{
			return Apply((Bitmap)image);
		}

		public Bitmap Apply(Bitmap image)
		{
			// lock source bitmap data
			BitmapData srcData = image.LockBits(
				new Rectangle(0, 0, image.Width, image.Height),
				ImageLockMode.ReadOnly, image.PixelFormat);

			Bitmap dstImage = null;

			try
			{
				// apply the filter
				dstImage = Apply(srcData);
				dstImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
			}
			finally
			{
				// unlock source image
				image.UnlockBits(srcData);
			}

			return dstImage;
		}

		public Bitmap Apply(BitmapData imageData)
		{
			// check pixel format of the source image
			CheckSourceFormat(imageData.PixelFormat);

			// get width and height
			int width = imageData.Width;
			int height = imageData.Height;

			// destination image format
			PixelFormat dstPixelFormat = FormatTranslations[imageData.PixelFormat];

			// create new image of required format
			Bitmap dstImage = (dstPixelFormat == PixelFormat.Format8bppIndexed) ?
				BitmapHelpers.CreateGrayscaleImage(width, height) :
				new Bitmap(width, height, dstPixelFormat);

			// lock destination bitmap data
			BitmapData dstData = dstImage.LockBits(
				new Rectangle(0, 0, width, height),
				ImageLockMode.ReadWrite, dstPixelFormat);

			try
			{
				// process the filter
				ProcessFilter(new UnsafeImage(imageData), new UnsafeImage(dstData));
			}
			finally
			{
				// unlock destination images
				dstImage.UnlockBits(dstData);
			}

			return dstImage;
		}

		public UnsafeImage Apply(UnsafeImage image)
		{
			// check pixel format of the source image
			CheckSourceFormat(image.PixelFormat);

			// create new destination image
			UnsafeImage dstImage = UnsafeImage.Create(image.Width, image.Height, FormatTranslations[image.PixelFormat]);

			// process the filter
			ProcessFilter(image, dstImage);

			return dstImage;
		}

		public void Apply(UnsafeImage srcImage, UnsafeImage dstImage)
		{
			// check pixel format of the source and destination images
			CheckSourceFormat(srcImage.PixelFormat);

			// ensure destination image has correct format
			if (dstImage.PixelFormat != FormatTranslations[srcImage.PixelFormat])
			{
				throw new InvalidPixelFormatException("Destination pixel format is specified incorrectly.");
			}

			// ensure destination image has correct size
			if ((dstImage.Width != srcImage.Width) || (dstImage.Height != srcImage.Height))
			{
				throw new InvalidImageException("Destination image must have the same width and height as source image.");
			}

			// process the filter
			ProcessFilter(srcImage, dstImage);
		}

		protected abstract unsafe void ProcessFilter(UnsafeImage srcData, UnsafeImage dstData);

		private void CheckSourceFormat(PixelFormat pixelFormat)
		{
			if (!FormatTranslations.ContainsKey(pixelFormat))
			{
				throw new InvalidPixelFormatException("Source pixel format is not supported by the filter.");
			}
		}
	}
}
