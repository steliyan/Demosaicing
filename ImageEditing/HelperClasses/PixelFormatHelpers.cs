using System;
using System.Drawing.Imaging;
using ImageEditing.Exceptions;

namespace ImageEditing.HelperClasses
{
	public static class PixelFormatHelpers
	{
		public static bool IsSupportedPixelFormat(this PixelFormat pixelFormat)
		{
			bool isSupportedPixelFormat =
				pixelFormat == PixelFormat.Format8bppIndexed ||
				pixelFormat == PixelFormat.Format16bppGrayScale ||
				pixelFormat == PixelFormat.Format24bppRgb ||
				pixelFormat == PixelFormat.Format32bppRgb ||
				pixelFormat == PixelFormat.Format32bppArgb ||
				pixelFormat == PixelFormat.Format32bppPArgb ||
				pixelFormat == PixelFormat.Format48bppRgb ||
				pixelFormat == PixelFormat.Format64bppArgb ||
				pixelFormat == PixelFormat.Format64bppPArgb;

			return isSupportedPixelFormat;
		}

		public static int GetBytesPerPixel(this PixelFormat pixelFormat)
		{
			if (!pixelFormat.IsSupportedPixelFormat())
			{
				throw new InvalidPixelFormatException("Unsupported pixel format.");
			}

			int bytesPerPixel = 0;

			switch (pixelFormat)
			{
				case PixelFormat.Format8bppIndexed:
					bytesPerPixel = 1;
					break;

				case PixelFormat.Format16bppGrayScale:
					bytesPerPixel = 2;
					break;

				case PixelFormat.Format24bppRgb:
					bytesPerPixel = 3;
					break;

				case PixelFormat.Format32bppRgb:
				case PixelFormat.Format32bppArgb:
				case PixelFormat.Format32bppPArgb:
					bytesPerPixel = 4;
					break;

				case PixelFormat.Format48bppRgb:
					bytesPerPixel = 6;
					break;

				case PixelFormat.Format64bppArgb:
				case PixelFormat.Format64bppPArgb:
					bytesPerPixel = 8;
					break;
			}

			return bytesPerPixel;
		}
	}
}
