using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using ImageEditing.Exceptions;

namespace ImageEditing.HelperClasses
{
	public static class BitmapHelpers
	{
		public static Image ResizeImage(Bitmap bitmap, Size size, bool preserveAspectRatio = true)
		{
			return ResizeImage((Image)bitmap, size, preserveAspectRatio);
		}

		public static Image ResizeImage(Image image, Size size, bool preserveAspectRatio = true)
		{
			if (size.Width <= 0 || size.Height <= 0)
			{
				return null;
			}

			int newWidth;
			int newHeight;

			if (preserveAspectRatio)
			{
				int srcWidth = image.Width;
				int srcHeight = image.Height;

				double percentWidth = (double)size.Width / srcWidth;
				double percentHeight = (double)size.Height / srcHeight;
				double percent =
					percentHeight < percentWidth ?
					percentHeight : percentWidth;

				newWidth = (int)(srcWidth * percent);
				newHeight = (int)(srcHeight * percent);
			}
			else
			{
				newWidth = size.Width;
				newHeight = size.Height;
			}

			Image dstImage = new Bitmap(newWidth, newHeight);
			using (Graphics graphicsHandle = Graphics.FromImage(dstImage))
			{
				graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight);
			}

			return dstImage;
		}

		public static Bitmap CreateGrayscaleImage(int width, int height)
		{
			// create new image
			Bitmap image = new Bitmap(width, height, PixelFormat.Format8bppIndexed);

			// set palette to grayscale
			SetGrayscalePalette(image);

			// return new image
			return image;
		}

		public static void SetGrayscalePalette(Bitmap bitmap)
		{
			// check pixel format
			if (bitmap.PixelFormat != PixelFormat.Format8bppIndexed)
			{
				throw new InvalidPixelFormatException("Source image is not 8 bpp image.");
			}

			// get palette
			ColorPalette colorPalette = bitmap.Palette;

			// init palette
			for (int i = 0; i < 256; i++)
			{
				colorPalette.Entries[i] = Color.FromArgb(i, i, i);
			}

			// set palette back
			bitmap.Palette = colorPalette;
		}
	}
}
