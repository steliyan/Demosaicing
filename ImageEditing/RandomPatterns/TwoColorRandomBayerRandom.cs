using System;
using ImageEditing.BaseInterfaces;
using ImageEditing.HelperClasses;

namespace ImageEditing.RandomPatterns
{
	public class TwoColorRandomBayerRandom : IRandomBayerPattern
	{
		private static readonly Random rand = new Random();

		// bayer pattern
		private bool isBayerPatternGenerated = false;
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
			get { return isBayerPatternGenerated; }
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
			if (isSizeDefined && !isBayerPatternGenerated)
			{
				FillBayerPattern();
				isBayerPatternGenerated = true;
			}
		}

		public TwoColorRandomBayerRandom()
		{ }

		public TwoColorRandomBayerRandom(int width, int height)
		{
			this.isSizeDefined = true;
			int widthOffset = width & 1;
			int heightOffset = height & 1;
			this.width = width + widthOffset;
			this.height = height + heightOffset;
			this.bayerPattern = new short[height, width];
		}

		public string BayerPatternShortName
		{
			get { return "TWOCOLOR"; }
		}

		public override string ToString()
		{
			return "Two Color Random Bayer";
		}

		private void FillBayerPattern()
		{
			for (int y = 0; y < height - 1; y += 2)
			{
				for (int x = 0; x < width - 1; x += 2)
				{
					int twoColorBayerPatternsCount = BayerPatternConstants.AllTwoColorBayerPatterns.Length;
					int randomIndex = rand.Next(0, twoColorBayerPatternsCount);
					short[,] randomBayerPattern = BayerPatternConstants.AllTwoColorBayerPatterns[randomIndex];

					bayerPattern[y, x] = randomBayerPattern[0, 0];
					bayerPattern[y + 1, x] = randomBayerPattern[1, 0];
					bayerPattern[y, x + 1] = randomBayerPattern[0, 1];
					bayerPattern[y + 1, x + 1] = randomBayerPattern[1, 1];
				}
			}
		}
	}
}
