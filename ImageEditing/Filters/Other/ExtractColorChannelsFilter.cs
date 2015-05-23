using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using ImageEditing.BaseClasses;
using ImageEditing.HelperClasses;

namespace ImageEditing.Filters
{
	public class ExtractColorChannelsFilter : BaseFilter
	{
		private readonly short[] colorChannels;
		private readonly Dictionary<PixelFormat, PixelFormat> formatTranslations = new Dictionary<PixelFormat, PixelFormat>();

		public short[] ColorChannel
		{
			get { return colorChannels; }
		}

		public override Dictionary<PixelFormat, PixelFormat> FormatTranslations
		{
			get { return formatTranslations; }
		}

		public ExtractColorChannelsFilter(short[] colorChannels)
		{
			this.colorChannels = colorChannels;

			// initialize format translation dictionary
			formatTranslations[PixelFormat.Format24bppRgb] = PixelFormat.Format24bppRgb;
			formatTranslations[PixelFormat.Format32bppRgb] = PixelFormat.Format32bppRgb;
			formatTranslations[PixelFormat.Format32bppArgb] = PixelFormat.Format32bppArgb;
		}

		public ExtractColorChannelsFilter(List<short> colorChannels)
			: this(colorChannels.ToArray())
		{}

		protected override unsafe void ProcessFilter(UnsafeImage srcData, UnsafeImage dstData)
		{
			// we want the full-color image
			if (
				ColorChannel.Contains(RGB.R) &&
				ColorChannel.Contains(RGB.G) &&
				ColorChannel.Contains(RGB.B))
			{
				srcData.Copy(dstData);
				return;
			}

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
					if(ColorChannel.Contains(RGB.R))
					{
						dst[RGB.R] = src[RGB.R];
					}

					if(ColorChannel.Contains(RGB.G))
					{
						dst[RGB.G] = src[RGB.G];
					}

					if(ColorChannel.Contains(RGB.B))
					{
						dst[RGB.B] = src[RGB.B];
					}
				}

				src += srcOffset;
				dst += dstOffset;
			}
		}
	}
}
