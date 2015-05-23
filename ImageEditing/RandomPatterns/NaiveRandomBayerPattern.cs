using System;
using ImageEditing.BaseClasses;
using ImageEditing.BaseInterfaces;

namespace ImageEditing.RandomPatterns
{
	public class NaiveRandomBayerPattern : IRandomBayerPattern
	{
		private static readonly Random rand = new Random();
		private static readonly double threshold1 = 0.33;
		private static readonly double threshold2 = 0.66;

		// bayer pattern
		private bool isPatternGenerated = false;
		private readonly bool isSizeDefined = false;
		private readonly int width;
		private readonly int height;
		private readonly short[,] bayerPattern;

		public short[,] BayerPattern
		{
			get { return bayerPattern; }
		}

		public bool IsBayerPatternGenerated
		{
			get { return isPatternGenerated; }
		}

		public bool IsSizeDefined
		{
			get { return isSizeDefined; }
		}

		public int Width
		{
			get { return width; }
		}

		public int Height
		{
			get { return height; }
		}

		public void GenerateBayerPattern()
		{
			if (isSizeDefined && !isPatternGenerated)
			{
				FillBayerPattern();
				isPatternGenerated = true;
			}
		}

		public NaiveRandomBayerPattern()
		{ }

		public NaiveRandomBayerPattern(int width, int height)
		{
			this.isSizeDefined = true;
			this.width = width;
			this.height = height;
			this.bayerPattern = new short[height, width];
		}

		public string BayerPatternShortName
		{
			get { return "NAIVE"; }
		}

		public override string ToString()
		{
			return "Naive Random Bayer";
		}

		private void FillBayerPattern()
		{
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					double randomNumber = rand.NextDouble();

					if (randomNumber <= threshold1)
					{
						bayerPattern[y, x] = RGB.R;
					}
					else if (randomNumber <= threshold2)
					{
						bayerPattern[y, x] = RGB.G;
					}
					else
					{
						bayerPattern[y, x] = RGB.B;
					}
				}
			}
		}
	}
}
