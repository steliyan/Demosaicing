using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using ImageEditing.BaseClasses;
using ImageEditing.BaseInterfaces;
using ImageEditing.HelperClasses;

namespace ImageEditing.Filters
{
	// TODO: Fix EdgeDemosaicing algorithm (check applying of sobel filter indeces)
	public class EdgeDemosaicing : BaseFilter, IDemosaicer
	{
		// extracted color channel images
		private UnsafeImage rColorUnsafeImage;
		private UnsafeImage gColorUnsafeImage;
		private UnsafeImage bColorUnsafeImage;

		// binary edge maps
		private bool[,] rBinaryEdgeMapY;
		private bool[,] rBinaryEdgeMapX;
		private bool[,] gBinaryEdgeMapY;
		private bool[,] gBinaryEdgeMapX;
		private bool[,] bBinaryEdgeMapY;
		private bool[,] bBinaryEdgeMapX;

		// color channels/sobel filters
		private ExtractColorChannelFilter rColorChannelFilter;
		private ExtractColorChannelFilter gColorChannelFilter;
		private ExtractColorChannelFilter bColorChannelFilter;
		private SobelFilter sobelYFilter;
		private SobelFilter sobelXFilter;

		private readonly byte threshold = 10;
		private readonly Dictionary<PixelFormat, PixelFormat> formatTranslations = new Dictionary<PixelFormat, PixelFormat>();

		public override Dictionary<PixelFormat, PixelFormat> FormatTranslations
		{
			get { return formatTranslations; }
		}

		public string DemosaicerShortname
		{
			get { return "EDGE1"; }
		}

		public override string ToString()
		{
			return "Edge Demosaicer";
		}

		public EdgeDemosaicing()
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

			// init sobel y/x filters
			sobelYFilter = new SobelFilter(PatternConstants.SobelPatternY5, threshold);
			sobelXFilter = new SobelFilter(PatternConstants.SobelPatternX5, threshold);
		}

		private void ExtractColorChannels(UnsafeImage srcData)
		{
			rColorUnsafeImage = rColorChannelFilter.Apply(srcData);
			gColorUnsafeImage = gColorChannelFilter.Apply(srcData);
			bColorUnsafeImage = bColorChannelFilter.Apply(srcData);
		}

		private void ApplySobelFilters()
		{
			// red channel
			sobelYFilter.Apply(rColorUnsafeImage);
			rBinaryEdgeMapY = sobelYFilter.BinaryEdgeMap;
			sobelXFilter.Apply(rColorUnsafeImage);
			rBinaryEdgeMapX = sobelXFilter.BinaryEdgeMap;

			// green channel
			sobelYFilter.Apply(gColorUnsafeImage);
			gBinaryEdgeMapY = sobelYFilter.BinaryEdgeMap;
			sobelXFilter.Apply(gColorUnsafeImage);
			gBinaryEdgeMapX = sobelXFilter.BinaryEdgeMap;

			// blue channel
			sobelYFilter.Apply(bColorUnsafeImage);
			bBinaryEdgeMapY = sobelYFilter.BinaryEdgeMap;
			sobelXFilter.Apply(bColorUnsafeImage);
			bBinaryEdgeMapX = sobelXFilter.BinaryEdgeMap;
		}

		private unsafe void InterpolateColorPlane(UnsafeImage srcData, UnsafeImage dstData, bool[,] binaryEdgeMapY, bool[,] binaryEdgeMapX, short colorChannel)
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

			// align destination pointer
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

					// we know this pixel's value
					if (*src != 0)
					{
						*dst = *src;
						continue;
					}

					// define our neighbour pixels
					byte v1 = CommonHelpers.GetPixelAt(src, srcStride, -1, 0);
					byte v2 = CommonHelpers.GetPixelAt(src, srcStride, +1, 0);
					byte v3 = CommonHelpers.GetPixelAt(src, srcStride, -2, 0);
					byte v4 = CommonHelpers.GetPixelAt(src, srcStride, +2, 0);
					byte h1 = CommonHelpers.GetPixelAt(src, srcStride, 0, +1);
					byte h2 = CommonHelpers.GetPixelAt(src, srcStride, 0, -1);
					byte h3 = CommonHelpers.GetPixelAt(src, srcStride, 0, +2);
					byte h4 = CommonHelpers.GetPixelAt(src, srcStride, 0, -2);

					bool isOnVerticalEdge = binaryEdgeMapY[y, x];
					bool isOnHorizontalEdge = binaryEdgeMapX[y, x];
					bool isOnBothEdges = isOnVerticalEdge && isOnHorizontalEdge;

					bool isMissingPixel = false;
					byte pixelValue = 0;
					if (isOnBothEdges)
					{
						if (v1 != 0) pixelValue = v1;
						else if (h1 != 0) pixelValue = h1;
						else if (v2 != 0) pixelValue = v2;
						else if (h2 != 0) pixelValue = h2;
						else if (v3 != 0) pixelValue = v3;
						else if (h3 != 0) pixelValue = h3;
						else if (v4 != 0) pixelValue = v4;
						else if (h4 != 0) pixelValue = h4;
						else isMissingPixel = true;
					}
					else if (isOnVerticalEdge)
					{
						if (v1 != 0) pixelValue = v1;
						else if (v2 != 0) pixelValue = v2;
						else if (v3 != 0) pixelValue = v3;
						else if (v4 != 0) pixelValue = v4;
						else isMissingPixel = true;
					}
					else if (isOnHorizontalEdge)
					{
						if (h1 != 0) pixelValue = h1;
						else if (h2 != 0) pixelValue = h2;
						else if (h3 != 0) pixelValue = h3;
						else if (h4 != 0) pixelValue = h4;
						else isMissingPixel = true;
					}

					// we didn't interpolate or couldn't interpolate this pixel value
					if (!isOnBothEdges || isMissingPixel)
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
			ApplySobelFilters();
			InterpolateColorPlane(rColorUnsafeImage, dstData, rBinaryEdgeMapY, rBinaryEdgeMapX, RGB.R);
			InterpolateColorPlane(gColorUnsafeImage, dstData, gBinaryEdgeMapY, gBinaryEdgeMapX, RGB.G);
			InterpolateColorPlane(bColorUnsafeImage, dstData, bBinaryEdgeMapY, bBinaryEdgeMapX, RGB.B);
		}
	}
}
