using System;
using ImageEditing.BaseClasses;
using ImageEditing.Exceptions;

namespace ImageEditing.HelperClasses
{
	public static class CommonHelpers
	{
		public static bool IsInRange(int y, int x, int height, int width)
		{
			return
				y >= 0 &&
				y < height &&
				x >= 0 &&
				x < width;
		}

		public static unsafe byte GetPixelAt(byte* ptr, int stride, int dy, int dx)
		{
			return *GetPixelPtrAt(ptr, stride, dy, dx);
		}

		public static unsafe byte* GetPixelPtrAt(byte* ptr, int stride, int dy, int dx)
		{
			return ptr + (stride * dy + dx);
		}

		public static unsafe BayerColor GetBayerColor(byte* ptr, int dy, int dx)
		{
			var r = ptr[RGB.R];
			var g = ptr[RGB.G];
			var b = ptr[RGB.B];

			if (
				(r != 0 && g != 0) ||
				(r != 0 && b != 0) ||
				(g != 0 && b != 0))
			{
				throw new InvalidImageException("This image is not supported!");
			}

			BayerColor bayerColor = new BayerColor(0, 0);
			if (r != 0 && g == 0 && b == 0)
			{
				bayerColor = new BayerColor(RGB.R, r);
			}
			else if (r == 0 && g != 0 && b == 0)
			{
				bayerColor = new BayerColor(RGB.G, g);
			}
			else if (r == 0 && g == 0 && b != 0)
			{
				bayerColor = new BayerColor(RGB.B, b);
			}

			return bayerColor;
		}

		public static short GetMissingThirdColorIndex(short firstColorIndex, short secondColorIndex)
		{
			if ((firstColorIndex == RGB.R && secondColorIndex == RGB.G) || (firstColorIndex == RGB.G && secondColorIndex == RGB.R))
			{
				return RGB.B;
			}

			if ((firstColorIndex == RGB.R && secondColorIndex == RGB.B) || (firstColorIndex == RGB.B && secondColorIndex == RGB.R))
			{
				return RGB.G;
			}

			if ((firstColorIndex == RGB.G && secondColorIndex == RGB.B) || (firstColorIndex == RGB.B && secondColorIndex == RGB.G))
			{
				return RGB.R;
			}

			throw new ArgumentException("FirstColor and SecondColor should be different!");
		}

		public static short[,] ToBayerArray(string bayerPattern)
		{
			short[,] arrayBayernPattern = new short[2, 2];

			for (int i = 0; i < 2; i++)
			{
				for (int j = 0; j < 2; j++)
				{
					int index = i * 2 + j;

					switch (bayerPattern[index])
					{
						case 'r':
						case 'R':
							arrayBayernPattern[i, j] = RGB.R;
							break;

						case 'g':
						case 'G':
							arrayBayernPattern[i, j] = RGB.G;
							break;

						case 'b':
						case 'B':
							arrayBayernPattern[i, j] = RGB.B;
							break;

						default:
							throw new ArgumentOutOfRangeException("Bayer pattern should only costists of RGB values!");
					}
				}
			}

			return arrayBayernPattern;
		}
	}
}
