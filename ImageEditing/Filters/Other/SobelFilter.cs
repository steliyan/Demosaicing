using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using ImageEditing.BaseClasses;
using ImageEditing.HelperClasses;

namespace ImageEditing.Filters
{
	public class SobelFilter : BaseFilter
	{
		private readonly short[,] sobelPattern = PatternConstants.SobelPatternX7;
		private readonly Dictionary<PixelFormat, PixelFormat> formatTranslations = new Dictionary<PixelFormat, PixelFormat>();
		private bool[,] binaryEdgeMap;
		private readonly byte threshold = 0;

		public short[,] SobelPattern
		{
			get { return sobelPattern; }
		}

		public override Dictionary<PixelFormat, PixelFormat> FormatTranslations
		{
			get { return formatTranslations; }
		}

		public bool[,] BinaryEdgeMap
		{
			get { return binaryEdgeMap; }
		}

		public byte Threshold
		{
			get { return threshold; }
		}

		public SobelFilter()
		{
			// initialize format translation dictionary
			formatTranslations[PixelFormat.Format8bppIndexed] = PixelFormat.Format8bppIndexed;
		}

		public SobelFilter(short[,] sobelPattern, byte threshold)
			: this()
		{
			this.sobelPattern = sobelPattern;
			this.threshold = threshold;
		}

		protected override unsafe void ProcessFilter(UnsafeImage srcData, UnsafeImage dstData)
		{
			// sobel filter dimensions
			int sobelWidth = sobelPattern.GetLength(1);
			int sobelHeight = sobelPattern.GetLength(0);
			int halfSobelWidth = sobelWidth / 2;
			int halfSobelHeight = sobelHeight / 2;

			int sobelStartX = -halfSobelWidth;
			int sobelStartY = -halfSobelHeight;
			int sobelStopX = halfSobelWidth;
			int sobelStopY = halfSobelHeight;

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

			// init edge map
			binaryEdgeMap = new bool[height, width];

			// for each line
			for (int y = 0; y < height; y++)
			{
				// for each pixel
				for (int x = 0; x < width; x++, src += srcPixelSize, dst += dstPixelSize)
				{
					double sumSobel = 0;
					for (int dy = sobelStartY; dy <= sobelStopY; dy++)
					{
						for (int dx = sobelStartX; dx <= sobelStopX; dx++)
						{
							if (CommonHelpers.IsInRange(y + dy, x + dx, height, width))
							{
								sumSobel += CommonHelpers.GetPixelAt(src, srcStride, dy, dx) * sobelPattern[dy + halfSobelHeight, dx + halfSobelWidth];
							}
						}
					}

					*dst = (byte)Math.Min(255, Math.Abs(sumSobel));

					// fill our binary edge map
					if (*dst >= threshold)
					{
						binaryEdgeMap[y, x] = true;
					}
				}

				src += srcOffset;
				dst += dstOffset;
			}
		}
	}
}
