using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using ImageEditing.BaseClasses;
using ImageEditing.BaseInterfaces;
using ImageEditing.HelperClasses;

namespace ImageEditing.Filters
{
	public class MhcDemosaicing : BaseFilter, IDemosaicer
	{
		private short[,] bayerPattern = PatternConstants.DefaultBayerPattern;
		private readonly Dictionary<PixelFormat, PixelFormat> formatTranslations = new Dictionary<PixelFormat, PixelFormat>();

		private int redY;
		private int redX;
		private int blueY;
		private int blueX;

		public short[,] BayerPattern
		{
			get { return bayerPattern; }
			set
			{
				this.bayerPattern = value;
				this.SetBayerIndeces();
			}
		}

		public override Dictionary<PixelFormat, PixelFormat> FormatTranslations
		{
			get { return formatTranslations; }
		}

		public string DemosaicerShortname
		{
			get { return "MHC"; }
		}

		public override string ToString()
		{
			return "Malvar-He-Cutler";
		}

		public MhcDemosaicing()
		{
			this.SetBayerIndeces();

			// initialize format translation dictionary
			formatTranslations[PixelFormat.Format24bppRgb] = PixelFormat.Format24bppRgb;
			formatTranslations[PixelFormat.Format32bppRgb] = PixelFormat.Format32bppRgb;
			formatTranslations[PixelFormat.Format32bppArgb] = PixelFormat.Format32bppArgb;
			formatTranslations[PixelFormat.Format32bppPArgb] = PixelFormat.Format32bppPArgb;
		}

		public MhcDemosaicing(short[,] bayerPattern)
			: this()
		{
			this.bayerPattern = bayerPattern;
			this.SetBayerIndeces();
		}

		public MhcDemosaicing(string bayerPattern)
			: this(CommonHelpers.ToBayerArray(bayerPattern))
		{ }

		private void SetBayerIndeces()
		{
			for (int y = 0; y < 2; y++)
			{
				for (int x = 0; x < 2; x++)
				{
					if (bayerPattern[y, x] == RGB.R)
					{
						redY = y;
						redX = x;
					}
					else if (bayerPattern[y, x] == RGB.B)
					{
						blueY = y;
						blueX = x;
					}
				}
			}
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

			// 5x5 adjacency matrices
			double[,] adj = new double[5, 5];
			short[,] adjPresence = new short[5, 5];

			// for each line
			for (int y = 0; y < height; y++)
			{
				// for each pixel
				for (int x = 0; x < width; x++, src += srcPixelSize, dst += dstPixelSize)
				{
					// copy neighbours
					for (int dy = -2; dy <= 2; dy++)
					{
						for (int dx = -2; dx <= 2; dx++)
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

								byte colorValue = bayerColor.ColorValue;

								adj[2 + dx, 2 + dy] = colorValue;
								adjPresence[2 + dx, 2 + dy] = 1;
							}
							else
							{
								adj[2 + dx, 2 + dy] = 0;
								adjPresence[2 + dx, 2 + dy] = 0;
							}
						}
					}

					// storing current bayer color we're working with
					int currentY = y & 1;
					int currentX = x & 1;

					// we're calculating these values, because they're symmetrical for red/blue channels
					var greenInterpolated = (2 * (adj[2, 1] + adj[1, 2] + adj[3, 2] + adj[2, 3]) + (adjPresence[0, 2] + adjPresence[4, 2] + adjPresence[2, 0] + adjPresence[2, 4]) * adj[2, 2] - adj[0, 2] - adj[4, 2] - adj[2, 0] - adj[2, 4]) / (2 * (adjPresence[2, 1] + adjPresence[1, 2] + adjPresence[3, 2] + adjPresence[2, 3]));
					var redBlueInterpolated = (4 * (adj[1, 1] + adj[3, 1] + adj[1, 3] + adj[3, 3]) + 3 * ((adjPresence[0, 2] + adjPresence[4, 2] + adjPresence[2, 0] + adjPresence[2, 4]) * adj[2, 2] - adj[0, 2] - adj[4, 2] - adj[2, 0] - adj[2, 4])) / (4 * (adjPresence[1, 1] + adjPresence[3, 1] + adjPresence[1, 3] + adjPresence[3, 3]));
					var redBlueNeighbour = (8 * (adj[1, 2] + adj[3, 2]) + (2 * (adjPresence[1, 1] + adjPresence[3, 1] + adjPresence[0, 2] + adjPresence[4, 2] + adjPresence[1, 3] + adjPresence[3, 3]) - adjPresence[2, 0] - adjPresence[2, 4]) * adj[2, 2] - 2 * (adj[1, 1] + adj[3, 1] + adj[0, 2] + adj[4, 2] + adj[1, 3] + adj[3, 3]) + adj[2, 0] + adj[2, 4]) / (8 * (adjPresence[1, 2] + adjPresence[3, 2]));
					var redBlueNonNeighbour = (8 * (adj[2, 1] + adj[2, 3]) + (2 * (adjPresence[1, 1] + adjPresence[3, 1] + adjPresence[2, 0] + adjPresence[2, 4] + adjPresence[1, 3] + adjPresence[3, 3]) - adjPresence[0, 2] - adjPresence[4, 2]) * adj[2, 2] - 2 * (adj[1, 1] + adj[3, 1] + adj[2, 0] + adj[2, 4] + adj[1, 3] + adj[3, 3]) + adj[0, 2] + adj[4, 2]) / (8 * (adjPresence[2, 1] + adjPresence[2, 3]));

					// vars for temp storage of interpolated color values
					double r = 0;
					double g = 0;
					double b = 0;

					if (currentY == redY && currentX == redX)
					{
						// center pixel is red
						r = src[RGB.R];
						g = greenInterpolated;
						b = redBlueInterpolated;
					}
					else if (currentY == blueY && currentX == blueX)
					{
						// center pixel is blue
						b = src[RGB.B];
						g = greenInterpolated;
						r = redBlueInterpolated;
					}
					else
					{
						// center pixel is green
						g = src[RGB.G];

						if (currentY == redY)
						{
							// left/right neighbours are red
							r = redBlueNeighbour;
							b = redBlueNonNeighbour;
						}
						else
						{
							// left/right neighbours are blue
							b = redBlueNeighbour;
							r = redBlueNonNeighbour;
						}
					}

					dst[RGB.R] = (byte)Math.Min(255, Math.Max(0, r));
					dst[RGB.G] = (byte)Math.Min(255, Math.Max(0, g));
					dst[RGB.B] = (byte)Math.Min(255, Math.Max(0, b));
				}

				src += srcOffset;
				dst += dstOffset;
			}
		}
	}
}
