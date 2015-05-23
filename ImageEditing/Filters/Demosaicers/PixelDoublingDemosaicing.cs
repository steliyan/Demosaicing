using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using ImageEditing.BaseClasses;
using ImageEditing.BaseInterfaces;
using ImageEditing.HelperClasses;

namespace ImageEditing.Filters
{
	public class PixelDoublingDemosaicing : BaseFilter, IDemosaicer
	{
		private readonly Dictionary<PixelFormat, PixelFormat> formatTranslations = new Dictionary<PixelFormat, PixelFormat>();

		public override Dictionary<PixelFormat, PixelFormat> FormatTranslations
		{
			get { return formatTranslations; }
		}

		public string DemosaicerShortname
		{
			get { return "PDOUBLING"; }
		}

		public override string ToString()
		{
			return "Pixel Doubling";
		}

		public PixelDoublingDemosaicing()
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
					// ensure we're not accessing external memory
					if (
						!CommonHelpers.IsInRange(y, x, height, width) ||
						!CommonHelpers.IsInRange(y + 2, x + 2, height, width))
					{
						continue;
					}

					foreach (var colorChannel in RGB.ColorChannels)
					{
						// perform pixel doubling demosaicing
						for (int dy = 0; dy <= 2; dy++)
						{
							bool found = false;
							for (int dx = 0; dx <= 2; dx++)
							{
								// our source image is not a grayscale, so we should
								// offset our x-direction ptr (we don't require the applied bayer pattern!)
								// [R|G|B] [R|G|B]...
								// |pixel| |pixel|...
								int xOffset = dx * srcPixelSize;

								if (!found && CommonHelpers.IsInRange(y + dy, x + xOffset, height, width))
								{
									byte* pixelPtr = CommonHelpers.GetPixelPtrAt(src, srcStride, dy, xOffset);
									BayerColor bayerColor = CommonHelpers.GetBayerColor(pixelPtr, dy, xOffset);

									short bayerIndex = bayerColor.ColorIndex;
									byte colorValue = bayerColor.ColorValue;

									if (bayerIndex == colorChannel)
									{
										dst[colorChannel] = colorValue;
										found = true;
									}
								}
							}
						}
					}
				}

				src += srcOffset;
				dst += dstOffset;
			}
		}
	}
}
