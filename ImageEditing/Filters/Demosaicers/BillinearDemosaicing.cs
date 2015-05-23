using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using ImageEditing.BaseClasses;
using ImageEditing.BaseInterfaces;
using ImageEditing.HelperClasses;

namespace ImageEditing.Filters
{
	public class BillinearDemosaicing : BaseFilter, IDemosaicer
	{
		private readonly Dictionary<PixelFormat, PixelFormat> formatTranslations = new Dictionary<PixelFormat, PixelFormat>();

		public override Dictionary<PixelFormat, PixelFormat> FormatTranslations
		{
			get { return formatTranslations; }
		}

		public string DemosaicerShortname
		{
			get { return "BIL"; }
		}

		public override string ToString()
		{
			return "Billinear";
		}

		public BillinearDemosaicing()
		{
			// initialize format translation dictionary
			formatTranslations[PixelFormat.Format24bppRgb] = PixelFormat.Format24bppRgb;
			formatTranslations[PixelFormat.Format32bppRgb] = PixelFormat.Format32bppRgb;
			formatTranslations[PixelFormat.Format32bppArgb] = PixelFormat.Format32bppArgb;
			formatTranslations[PixelFormat.Format32bppPArgb] = PixelFormat.Format32bppPArgb;
		}

		protected override unsafe void ProcessFilter(UnsafeImage srcData, UnsafeImage dstData)
		{
			// get source/destination sizes, strides, pixelsizes, offsets, etc..
			int width = srcData.Width;
			int height = srcData.Height;

			int srcStride = srcData.Stride;
			int dstStride = dstData.Stride;

			int srcPixelSize = PixelFormatHelpers.GetBytesPerPixel(srcData.PixelFormat);
			int dstPixelSize = PixelFormatHelpers.GetBytesPerPixel(dstData.PixelFormat);

			int srcOffset = srcStride - width * srcPixelSize;
			int dstOffset = dstStride - width * dstPixelSize;

			// get source/destination pointers
			byte* src = (byte*)srcData.ImageData.ToPointer();
			byte* dst = (byte*)dstData.ImageData.ToPointer();

			// for each line
			for (int y = 0; y < height; y++)
			{
				// for each pixel
				for (int x = 0; x < width; x++, src += srcPixelSize, dst += dstPixelSize)
				{
					int[] rgbValues = { 0, 0, 0 };
					int[] rgbCounters = { 0, 0, 0 };

					// perform billinear demosaicing
					for (int dy = -1; dy <= 1; dy++)
					{
						for (int dx = -1; dx <= 1; dx++)
						{
							// our source image is not a grayscale, so we should
							// offset our x-direction ptr (we don't require the applied bayer pattern!)
							// [R|G|B] [R|G|B]...
							// |pixel| |pixel|...
							int xOffset = dx * srcPixelSize;

							if (CommonHelpers.IsInRange(y + dy, x + xOffset, height, width))
							{
								byte* pixelPtr = CommonHelpers.GetPixelPtrAt(src, srcStride, dy, xOffset);
								BayerColor bayerColor = CommonHelpers.GetBayerColor(pixelPtr, dy, xOffset);

								int bayerIndex = bayerColor.ColorIndex;
								byte colorValue = bayerColor.ColorValue;

								if (colorValue != 0)
								{
									rgbValues[bayerIndex] += colorValue;
									rgbCounters[bayerIndex]++;
								}
							}
						}
					}

					if (dst[RGB.R] == 0 && rgbCounters[RGB.R] != 0)
					{
						dst[RGB.R] = (byte)(rgbValues[RGB.R] / rgbCounters[RGB.R]);
					}

					if (dst[RGB.G] == 0 && rgbCounters[RGB.G] != 0)
					{
						dst[RGB.G] = (byte)(rgbValues[RGB.G] / rgbCounters[RGB.G]);
					}

					if (dst[RGB.B] == 0 && rgbCounters[RGB.B] != 0)
					{
						dst[RGB.B] = (byte)(rgbValues[RGB.B] / rgbCounters[RGB.B]);
					}
				}

				src += srcOffset;
				dst += dstOffset;
			}
		}
	}
}
