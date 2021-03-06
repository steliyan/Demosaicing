﻿using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using ImageEditing.BaseClasses;
using ImageEditing.BaseInterfaces;
using ImageEditing.HelperClasses;

namespace ImageEditing.Filters
{
	public class BayerFilter : BaseFilter, IDemosaicable
	{
		private short[,] bayerPattern = PatternConstants.DefaultBayerPattern;
		private readonly Dictionary<PixelFormat, PixelFormat> formatTranslations = new Dictionary<PixelFormat, PixelFormat>();
		private readonly List<IDemosaicer> demosaicers = new List<IDemosaicer>();

		public short[,] BayerPattern
		{
			get { return bayerPattern; }
			set
			{
				bayerPattern = value;
			}
		}

		public override Dictionary<PixelFormat, PixelFormat> FormatTranslations
		{
			get { return formatTranslations; }
		}

		public List<IDemosaicer> Demosaicers
		{
			get { return demosaicers; }
		}

		public BayerFilter()
		{
			demosaicers.Add(new PixelDoublingDemosaicing());
			demosaicers.Add(new BillinearDemosaicing());
			demosaicers.Add(new LinearDemosaicing());
			demosaicers.Add(new CubicDemosaicing());
			demosaicers.Add(new EdgeDemosaicing());
			demosaicers.Add(new NewEdgeDemosaicing());
			demosaicers.Add(new MhcDemosaicing());
	
			// initialize format translation dictionary
			formatTranslations[PixelFormat.Format8bppIndexed] = PixelFormat.Format24bppRgb;
		}

		public BayerFilter(short[,] bayerPattern)
			: this()
		{
			this.bayerPattern = bayerPattern;
		}

		public BayerFilter(string bayerPattern)
			: this(CommonHelpers.ToBayerArray(bayerPattern))
		{ }

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
					int bayerIndex = bayerPattern[y & 1, x & 1];

					dst[RGB.R] = dst[RGB.G] = dst[RGB.B] = 0;
					dst[bayerIndex] = CommonHelpers.GetPixelAt(src, srcStride, 0, 0);
				}

				src += srcOffset;
				dst += dstOffset;
			}
		}
	}
}
