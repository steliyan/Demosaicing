using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using ImageEditing.BaseClasses;
using ImageEditing.HelperClasses;

namespace ImageEditing.Filters
{
	public class BayerImage
	{
		private const PixelFormat pixelFormat = PixelFormat.Format24bppRgb;
		private readonly int pixelSize = PixelFormatHelpers.GetBytesPerPixel(pixelFormat);
		private readonly int tileSize;
		private readonly bool alignPattern;
		private readonly short[,] bayerPattern;

		private int width;
		private int height;
		private Bitmap bayerImage;

		public Bitmap Image
		{
			get { return bayerImage; }
		}

		public BayerImage(short[,] bayerPattern, int width, int height, int tileSize = 10, bool alignPattern = true)
		{
			this.bayerPattern = bayerPattern;
			this.width = width;
			this.height = height;
			this.tileSize = tileSize;
			this.alignPattern = alignPattern;
			this.ClipSize();
			this.GenerateBayerImage();
		}

		private void ClipSize()
		{
			if (alignPattern)
			{
				int clippedWidth = width - (width % tileSize);
				int clippedHeight = height - (height % tileSize);
				width = clippedWidth;
				height = clippedHeight;
			}
		}

		private void GenerateBayerImage()
		{
			bayerImage = new Bitmap(width, height, pixelFormat);
			BitmapData bayerData = bayerImage.LockBits(
				new Rectangle(0, 0, width, height),
				ImageLockMode.WriteOnly, pixelFormat);

			try
			{
				FillBayerImage(new UnsafeImage(bayerData));
			}
			finally
			{
				bayerImage.UnlockBits(bayerData);
			}
		}

		private unsafe void FillBayerImage(UnsafeImage bayerData)
		{
			int stride = bayerData.Stride;
			int offset = stride - width * pixelSize;
			byte* ptr = (byte*)bayerData.ImageData.ToPointer();

			int maxWidth = bayerPattern.GetLength(1);
			int maxHeight = bayerPattern.GetLength(0);

			// for each line
			for (int y = 0; y < height; y++)
			{
				// for each pixel
				for (int x = 0; x < width; x++, ptr += pixelSize)
				{
					short bayerIndex = bayerPattern[(y / tileSize) % maxHeight, (x / tileSize) % maxWidth];
					ptr[bayerIndex] = byte.MaxValue;
				}

				ptr += offset;
			}
		}
	}
}
