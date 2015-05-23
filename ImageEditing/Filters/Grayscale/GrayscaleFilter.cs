using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using ImageEditing.BaseClasses;
using ImageEditing.HelperClasses;

namespace ImageEditing.Filters
{
	public class GrayscaleFilter : BaseFilter
	{
		public double RedCoefficient { get; private set; }
		public double GreenCoefficient { get; private set; }
		public double BlueCoefficient { get; private set; }

		public override Dictionary<PixelFormat, PixelFormat> FormatTranslations
		{
			get { return formatTranslations; }
		}

		private readonly Dictionary<PixelFormat, PixelFormat> formatTranslations = new Dictionary<PixelFormat, PixelFormat>();

		public GrayscaleFilter(double redCoefficient = 0.299, double greenCoefficient = 0.587, double blueCoefficient = 0.114)
		{
			// init rgb coefficients
			RedCoefficient = redCoefficient;
			GreenCoefficient = greenCoefficient;
			BlueCoefficient = blueCoefficient;

			// initialize format translation dictionary
			formatTranslations[PixelFormat.Format8bppIndexed] = PixelFormat.Format8bppIndexed;
			formatTranslations[PixelFormat.Format24bppRgb] = PixelFormat.Format8bppIndexed;
			formatTranslations[PixelFormat.Format32bppRgb] = PixelFormat.Format8bppIndexed;
			formatTranslations[PixelFormat.Format32bppArgb] = PixelFormat.Format8bppIndexed;
		}

		protected override unsafe void ProcessFilter(UnsafeImage srcData, UnsafeImage dstData)
		{
			if (srcData.PixelFormat == PixelFormat.Format8bppIndexed)
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
					double grayColor = 
						RedCoefficient * src[RGB.R] +
						GreenCoefficient * src[RGB.G] +
						BlueCoefficient * src[RGB.B];

					*dst = (byte)Math.Min(255, Math.Max(0, grayColor));
				}

				src += srcOffset;
				dst += dstOffset;
			}
		}
	}
}
