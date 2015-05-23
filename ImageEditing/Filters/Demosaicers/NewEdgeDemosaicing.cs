using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using ImageEditing.BaseClasses;
using ImageEditing.BaseInterfaces;
using ImageEditing.HelperClasses;

namespace ImageEditing.Filters
{
	public class NewEdgeDemosaicing : BaseFilter, IDemosaicer
	{
		// thresholds, size
		private const int Threshold = 40;
		private const int DeltaThreshold = 100;
		private int width;
		private int height;

		// extracted color channel images
		private UnsafeImage rColorUnsafeImage;
		private UnsafeImage gColorUnsafeImage;
		private UnsafeImage bColorUnsafeImage;

		// binary edge maps
		private DeltaPair[,] rBinaryEdgeMap;
		private DeltaPair[,] gBinaryEdgeMap;
		private DeltaPair[,] bBinaryEdgeMap;

		// color channels filters
		private ExtractColorChannelFilter rColorChannelFilter;
		private ExtractColorChannelFilter gColorChannelFilter;
		private ExtractColorChannelFilter bColorChannelFilter;

		private readonly Dictionary<PixelFormat, PixelFormat> formatTranslations = new Dictionary<PixelFormat, PixelFormat>();

		public override Dictionary<PixelFormat, PixelFormat> FormatTranslations
		{
			get { return formatTranslations; }
		}

		public string DemosaicerShortname
		{
			get { return "EDGE2"; }
		}

		public override string ToString()
		{
			return "New Edge Demosaicer";
		}

		public NewEdgeDemosaicing()
		{
			// initialize format translation dictionary
			formatTranslations[PixelFormat.Format24bppRgb] = PixelFormat.Format24bppRgb;
			formatTranslations[PixelFormat.Format32bppRgb] = PixelFormat.Format32bppRgb;
			formatTranslations[PixelFormat.Format32bppArgb] = PixelFormat.Format32bppArgb;
			formatTranslations[PixelFormat.Format32bppPArgb] = PixelFormat.Format32bppPArgb;
		}

		private void InitFilters()
		{
			// init color extractor filters
			rColorChannelFilter = new ExtractColorChannelFilter(RGB.R);
			gColorChannelFilter = new ExtractColorChannelFilter(RGB.G);
			bColorChannelFilter = new ExtractColorChannelFilter(RGB.B);
		}

		private void ExtractColorChannels(UnsafeImage srcData)
		{
			width = srcData.Width;
			height = srcData.Height;

			rColorUnsafeImage = rColorChannelFilter.Apply(srcData);
			gColorUnsafeImage = gColorChannelFilter.Apply(srcData);
			bColorUnsafeImage = bColorChannelFilter.Apply(srcData);
		}

		private void InitEdgeMaps()
		{
			rBinaryEdgeMap = new DeltaPair[height, width];
			gBinaryEdgeMap = new DeltaPair[height, width];
			bBinaryEdgeMap = new DeltaPair[height, width];
		}

		private unsafe void ConstructEdgeMap(UnsafeImage srcData, DeltaPair[,] binaryEdgeMap)
		{
			// get strides, pixelsizes, offsets, etc..
			int srcStride = srcData.Stride;

			int srcPixelSize = PixelFormatHelpers.GetBytesPerPixel(srcData.PixelFormat);

			int srcOffset = srcStride - width * srcPixelSize;

			// get source/destination pointers
			byte* src = (byte*)srcData.ImageData.ToPointer();

			// for each line
			for (int y = 0; y < height; y++)
			{
				// for each pixel
				for (int x = 0; x < width; x++, src += srcPixelSize)
				{
					// ensure we're not accessing external memory
					if (
						!CommonHelpers.IsInRange(y - 2, x - 2, height, width) ||
						!CommonHelpers.IsInRange(y + 2, x + 2, height, width))
					{
						continue;
					}

					int v1 = 0;
					int v2 = 0;
					int h1 = 0;
					int h2 = 0;
					for (int i = 0; i < 5; i++)
					{
						h1 += CommonHelpers.GetPixelAt(src, srcStride, i - 2, -1);
						h2 += CommonHelpers.GetPixelAt(src, srcStride, i - 2, +1);

						v1 += CommonHelpers.GetPixelAt(src, srcStride, -1, i - 2);
						v2 += CommonHelpers.GetPixelAt(src, srcStride, +1, i - 2);
					}

					int deltaH1 = Math.Abs(h1 - h2);
					int deltaH2 = (int)Math.Sqrt(Math.Abs(h1 * h1 - h2 * h2));
					int deltaV1 = Math.Abs(v1 - v2);
					int deltaV2 = (int)Math.Sqrt(Math.Abs(v1 * v2 - v2 * v2));
					binaryEdgeMap[y, x] = new DeltaPair(deltaH1, deltaH2, deltaV1, deltaV2);
				}

				src += srcOffset;
			}
		}

		private unsafe void InterpolateColorPlane(UnsafeImage srcData, UnsafeImage dstData, DeltaPair[,] binaryEdgeMap, short colorChannel)
		{
			// get strides, pixelsizes, offsets, etc..
			int srcStride = srcData.Stride;
			int dstStride = dstData.Stride;

			int srcPixelSize = PixelFormatHelpers.GetBytesPerPixel(srcData.PixelFormat);
			int dstPixelSize = PixelFormatHelpers.GetBytesPerPixel(dstData.PixelFormat);

			int srcOffset = srcStride - width * srcPixelSize;
			int dstOffset = dstStride - width * dstPixelSize;

			// get source/destination pointers
			byte* src = (byte*)srcData.ImageData.ToPointer();
			byte* dst = (byte*)dstData.ImageData.ToPointer();

			// align destination pointer to color channel
			dst += colorChannel;

			// for each line
			for (int y = 0; y < height; y++)
			{
				// for each pixel
				for (int x = 0; x < width; x++, src += srcPixelSize, dst += dstPixelSize)
				{
					// ensure we're not accessing external memory
					if (
						!CommonHelpers.IsInRange(y - 2, x - 2, height, width) ||
						!CommonHelpers.IsInRange(y + 2, x + 2, height, width))
					{
						continue;
					}

					int deltaH = binaryEdgeMap[y, x].DeltaH2;
					int deltaV = binaryEdgeMap[y, x].DeltaV2;

					// define our neighbour pixels
					byte v1 = CommonHelpers.GetPixelAt(src, srcStride, -1, 0);
					byte v2 = CommonHelpers.GetPixelAt(src, srcStride, +1, 0);
					byte v3 = CommonHelpers.GetPixelAt(src, srcStride, -2, 0);
					byte v4 = CommonHelpers.GetPixelAt(src, srcStride, +2, 0);
					byte h1 = CommonHelpers.GetPixelAt(src, srcStride, 0, +1);
					byte h2 = CommonHelpers.GetPixelAt(src, srcStride, 0, -1);
					byte h3 = CommonHelpers.GetPixelAt(src, srcStride, 0, +2);
					byte h4 = CommonHelpers.GetPixelAt(src, srcStride, 0, -2);

					byte pixelValue = 0;
					bool missingPixel = false;

					if (Math.Abs(deltaH - deltaV) <= DeltaThreshold)
					{
						if (deltaH > Threshold || deltaV > Threshold)
						{
							if (v1 != 0) pixelValue = v1;
							else if (h1 != 0) pixelValue = h1;
							else if (v2 != 0) pixelValue = v2;
							else if (h2 != 0) pixelValue = h2;
							else if (v3 != 0) pixelValue = v3;
							else if (h3 != 0) pixelValue = h3;
							else if (v4 != 0) pixelValue = v4;
							else if (h4 != 0) pixelValue = h4;
							else missingPixel = true;
						}
						else
						{
							int sum = 0;
							int count = 0;
							for (int dy = -1; dy <= 1; dy++)
							{
								for (int dx = -1; dx <= 1; dx++)
								{
									int adjPixelValue = CommonHelpers.GetPixelAt(src, srcStride, dy, dx);
									if (adjPixelValue != 0)
									{
										sum += adjPixelValue;
										count++;
									}
								}
							}

							if (count != 0)
							{
								pixelValue = (byte)(sum / count);
							}
						}
					}
					else
					{
						if (deltaH > deltaV)
						{
							if (h1 != 0) pixelValue = h1;
							else if (h2 != 0) pixelValue = h2;
							else if (h3 != 0) pixelValue = h3;
							else if (h4 != 0) pixelValue = h4;
							else missingPixel = true;
						}
						else
						{
							if (v1 != 0) pixelValue = v1;
							else if (v2 != 0) pixelValue = v2;
							else if (v3 != 0) pixelValue = v3;
							else if (v4 != 0) pixelValue = v4;
							else missingPixel = true;
						}
					}

					if (missingPixel)
					{
						int sum = 0;
						int count = 0;
						for (int dy = -1; dy <= 1; dy++)
						{
							for (int dx = -1; dx <= 1; dx++)
							{
								int adjPixelValue = CommonHelpers.GetPixelAt(src, srcStride, dy, dx);
								if (adjPixelValue != 0)
								{
									sum += adjPixelValue;
									count++;
								}
							}
						}

						if (count != 0)
						{
							pixelValue = (byte)(sum / count);
						}
					}

					*dst = pixelValue;
				}

				src += srcOffset;
				dst += dstOffset;
			}
		}

		protected override unsafe void ProcessFilter(UnsafeImage srcData, UnsafeImage dstData)
		{
			InitFilters();
			ExtractColorChannels(srcData);
			InitEdgeMaps();
			ConstructEdgeMap(rColorUnsafeImage, rBinaryEdgeMap);
			ConstructEdgeMap(gColorUnsafeImage, gBinaryEdgeMap);
			ConstructEdgeMap(bColorUnsafeImage, bBinaryEdgeMap);
			InterpolateColorPlane(rColorUnsafeImage, dstData, rBinaryEdgeMap, RGB.R);
			InterpolateColorPlane(gColorUnsafeImage, dstData, gBinaryEdgeMap, RGB.G);
			InterpolateColorPlane(bColorUnsafeImage, dstData, bBinaryEdgeMap, RGB.B);
		}
	}
}
