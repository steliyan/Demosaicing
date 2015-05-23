using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using ImageEditing.BaseClasses;
using ImageEditing.HelperClasses;

namespace ImageEditing.Filters
{
	public class GrayscaleBayer6X6Filter : BaseFilter
	{
		private readonly short[,] bayerPattern = PatternConstants.FujiBayerPattern;
		private readonly Dictionary<PixelFormat, PixelFormat> formatTranslations = new Dictionary<PixelFormat, PixelFormat>();

		public override Dictionary<PixelFormat, PixelFormat> FormatTranslations
		{
			get { return formatTranslations; }
		}

		public GrayscaleBayer6X6Filter()
		{
			// initialize format translation dictionary
			formatTranslations[PixelFormat.Format24bppRgb] = PixelFormat.Format8bppIndexed;
			formatTranslations[PixelFormat.Format32bppRgb] = PixelFormat.Format8bppIndexed;
			formatTranslations[PixelFormat.Format32bppArgb] = PixelFormat.Format8bppIndexed;
		}

		public GrayscaleBayer6X6Filter(short[,] bayerPattern)
			: this()
		{
			this.bayerPattern = bayerPattern;
		}

		protected override unsafe void ProcessFilter(UnsafeImage srcData, UnsafeImage dstData)
		{
			// get source/destination sizes, strides, pixelsizes, offsets, etc..
			int width = srcData.Width;
			int height = srcData.Height;

			int srcPixelSize = PixelFormatHelpers.GetBytesPerPixel(srcData.PixelFormat);
			int dstPixelSize = PixelFormatHelpers.GetBytesPerPixel(dstData.PixelFormat);

			int srcOffset = srcData.Stride - width * srcPixelSize;
			int dstOffset = dstData.Stride - width * dstPixelSize;

			// get source/destination pointers
			byte* src = (byte*)srcData.ImageData.ToPointer();
			byte* dst = (byte*)dstData.ImageData.ToPointer();

			// for each line
			for (int y = 0; y < height; y++)
			{
				// for each pixel
				for (int x = 0; x < width; x++, src += srcPixelSize, dst += dstPixelSize)
				{
					int bayerIndex = bayerPattern[y % 6, x % 6];
					*dst = src[bayerIndex];
				}

				src += srcOffset;
				dst += dstOffset;
			}
		}
	}
}
