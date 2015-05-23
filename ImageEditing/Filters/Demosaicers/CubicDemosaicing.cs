using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using ImageEditing.BaseClasses;
using ImageEditing.HelperClasses;
using ImageEditing.BaseInterfaces;

namespace ImageEditing.Filters
{
	public class CubicDemosaicing : BaseFilter, IDemosaicer
	{
		private readonly short[,] greenKernel = PatternConstants.GreenKernel7X7;
		private readonly short[,] redBlueKernel = PatternConstants.RedBlueKernel7X7;
		private readonly Dictionary<PixelFormat, PixelFormat> formatTranslations = new Dictionary<PixelFormat, PixelFormat>();

		public string DemosaicerShortname
		{
			get { return "CUB"; }
		}

		public override string ToString()
		{
			return "Cubic";
		}


		public override Dictionary<PixelFormat, PixelFormat> FormatTranslations
		{
			get { return formatTranslations; }
		}

		public CubicDemosaicing()
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
					int r = 0;
					int g = 0;
					int b = 0;

					for (int dy = -3; dy <= 3; dy++)
					{
						for (int dx = -3; dx <= 3; dx++)
						{
							// our source image is not a grayscale, so we should
							// offset our x-direction ptr (we don't require the applied bayer pattern!)
							// [R|G|B] [R|G|B]...
							// |pixel| |pixel|...
							int xOffset = dx * srcPixelSize;

							if (CommonHelpers.IsInRange(y + dy, x + xOffset, height, width))
							{
								r += CommonHelpers.GetPixelPtrAt(src, srcStride, dy, xOffset)[RGB.R] * redBlueKernel[dy + 3, dx + 3];
								g += CommonHelpers.GetPixelPtrAt(src, srcStride, dy, xOffset)[RGB.G] * greenKernel[dy + 3, dx + 3];
								b += CommonHelpers.GetPixelPtrAt(src, srcStride, dy, xOffset)[RGB.B] * redBlueKernel[dy + 3, dx + 3];
							}
						}
					}

					dst[RGB.R] = (byte)Math.Min(255, Math.Abs(r / 256));
					dst[RGB.G] = (byte)Math.Min(255, Math.Abs(g / 256));
					dst[RGB.B] = (byte)Math.Min(255, Math.Abs(b / 256));
				}

				src += srcOffset;
				dst += dstOffset;
			}
		}
	}
}
