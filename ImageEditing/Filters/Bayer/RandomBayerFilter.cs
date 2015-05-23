﻿using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using ImageEditing.BaseClasses;
using ImageEditing.BaseInterfaces;
using ImageEditing.HelperClasses;

namespace ImageEditing.Filters
{
	public class RandomBayerFilter : BaseFilter, IDemosaicable
	{
		private readonly short[,] bayerPattern;
		private readonly Dictionary<PixelFormat, PixelFormat> formatTranslations = new Dictionary<PixelFormat, PixelFormat>();
		private readonly List<IDemosaicer> demosaicers = new List<IDemosaicer>();

		public short[,] BayerPattern
		{
			get { return bayerPattern; }
		}

		public override Dictionary<PixelFormat, PixelFormat> FormatTranslations
		{
			get { return formatTranslations; }
		}
		
		public List<IDemosaicer> Demosaicers
		{
			get { return demosaicers; }
		}

		public RandomBayerFilter(short[,] bayerPattern)
		{
			demosaicers.Add(new PixelDoublingDemosaicing());
			demosaicers.Add(new BillinearDemosaicing());
			demosaicers.Add(new EdgeDemosaicing());
			demosaicers.Add(new NewEdgeDemosaicing());

			this.bayerPattern = bayerPattern;

			// initialize format translation dictionary
			formatTranslations[PixelFormat.Format8bppIndexed] = PixelFormat.Format24bppRgb;
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

			int srcOffset = srcStride - width;
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
					int bayerIndex = bayerPattern[y, x];

					dst[RGB.R] = dst[RGB.G] = dst[RGB.B] = 0;
					dst[bayerIndex] = CommonHelpers.GetPixelAt(src, srcStride, 0, 0);
				}

				src += srcOffset;
				dst += dstOffset;
			}
		}
	}
}
