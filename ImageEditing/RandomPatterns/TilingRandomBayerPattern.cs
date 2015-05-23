using System;
using System.Collections.Generic;
using System.Linq;
using ImageEditing.BaseClasses;
using ImageEditing.BaseInterfaces;
using ImageEditing.HelperClasses;

namespace ImageEditing.RandomPatterns
{
	public class TilingRandomBayerPattern : IRandomBayerPattern
	{
		private static readonly Random rand = new Random();

		// bayer pattern
		private bool isBayerPatternGenerated = false;
		private readonly bool isSizeDefined = false;
		private readonly int width;
		private readonly int height;
		private readonly short[,] bayerPattern;

		// color blocks
		private const int ColorBlockLength = 3;
		private readonly short[] rgb = { RGB.R, RGB.G, RGB.B };
		private readonly short[] rbg = { RGB.R, RGB.B, RGB.G };
		private readonly short[] gbr = { RGB.G, RGB.B, RGB.R };
		private readonly short[] grb = { RGB.G, RGB.R, RGB.B };
		private readonly short[] brg = { RGB.B, RGB.R, RGB.G };
		private readonly short[] bgr = { RGB.B, RGB.G, RGB.R };
		private readonly Dictionary<short[], HashSet<short[]>> adjacency = new Dictionary<short[], HashSet<short[]>>();

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
				InitAdjacencyRules();
				FillFirstRow();
				FillFirstColumn();
				FillBayerPattern();
				isBayerPatternGenerated = true;
			}
		}

		public TilingRandomBayerPattern()
		{ }

		public TilingRandomBayerPattern(int width, int height)
		{
			this.isSizeDefined = true;
			this.width = width;
			this.height = height;
			this.bayerPattern = new short[height, width];
		}

		public string BayerPatternShortName
		{
			get { return "TILING"; }
		}

		public override string ToString()
		{
			return "Tiling Random Bayer";
		}

		private void InitAdjacencyRules()
		{
			HashSet<short[]> adjRgb = new HashSet<short[]>();
			adjRgb.Add(rbg);
			adjRgb.Add(grb);
			adjacency.Add(rgb, adjRgb);

			HashSet<short[]> adjRbg = new HashSet<short[]>();
			adjRbg.Add(rgb);
			adjRbg.Add(brg);
			adjacency.Add(rbg, adjRbg);

			HashSet<short[]> adjGbr = new HashSet<short[]>();
			adjGbr.Add(grb);
			adjGbr.Add(bgr);
			adjacency.Add(gbr, adjGbr);

			HashSet<short[]> adjGrb = new HashSet<short[]>();
			adjGrb.Add(rgb);
			adjGrb.Add(gbr);
			adjacency.Add(grb, adjGrb);

			HashSet<short[]> adjBrg = new HashSet<short[]>();
			adjBrg.Add(rbg);
			adjBrg.Add(bgr);
			adjacency.Add(brg, adjBrg);

			HashSet<short[]> adjBgr = new HashSet<short[]>();
			adjBgr.Add(gbr);
			adjBgr.Add(brg);
			adjacency.Add(bgr, adjBgr);
		}

		private void FillFirstRow()
		{
			// calculate the needed initial blocks
			int blocksCount = (int)Math.Ceiling((double)width / ColorBlockLength);
			List<short[]> blocks = new List<short[]>();

			// initially "rgb" block
			int startRandomIndex = rand.Next(0, adjacency.Keys.Count);
			short[] startBlock = adjacency.ElementAt(startRandomIndex).Key;
			blocks.Add(startBlock);

			// fill the dummy list
			for (int i = 1; i < blocksCount; i++)
			{
				short[] prevBlock = blocks[i - 1];
				int randomIndex = rand.Next(0, adjacency[prevBlock].Count);
				short[] nextBlock = adjacency[prevBlock].ElementAt(randomIndex);
				blocks.Add(nextBlock);
			}

			// fill the main bayer cfa
			for (int i = 0; i < blocksCount; i++)
			{
				for (int rgbIndex = 0; rgbIndex < ColorBlockLength; rgbIndex++)
				{
					int bayerPatternIndex = i * ColorBlockLength + rgbIndex;

					if (bayerPatternIndex < width)
					{
						bayerPattern[0, bayerPatternIndex] = blocks[i][rgbIndex];
					}
				}
			}
		}

		private void FillFirstColumn()
		{
			// calculate the needed initial blocks
			int blocksCount = (int)Math.Ceiling((double)(height - 1) / ColorBlockLength);
			List<short[]> blocks = new List<short[]>();

			// we dont want overlapping colors
			var possibleBlocks = adjacency.Keys.Select(k => k).Where(k => k[0] != bayerPattern[0, 0]).ToList();
			int startRandomIndex = rand.Next(0, possibleBlocks.Count);
			short[] startBlock = possibleBlocks.ElementAt(startRandomIndex);
			blocks.Add(startBlock);

			// fill the dummy list
			for (int i = 1; i < blocksCount; i++)
			{
				short[] prevBlock = blocks[i - 1];
				int randomIndex = rand.Next(0, adjacency[prevBlock].Count);
				short[] nextBlock = adjacency[prevBlock].ElementAt(randomIndex);
				blocks.Add(nextBlock);
			}

			// fill the main bayer cfa
			for (int i = 0; i < blocksCount; i++)
			{
				for (int rgbIndex = 0; rgbIndex < ColorBlockLength; rgbIndex++)
				{
					int bayerPatternIndex = i * ColorBlockLength + rgbIndex + 1;

					if (bayerPatternIndex < height)
					{
						bayerPattern[bayerPatternIndex, 0] = blocks[i][rgbIndex];
					}
				}
			}
		}

		private void FillBayerPattern()
		{
			for (int y = 1; y < height; y++)
			{
				for (int x = 1; x < width; x++)
				{
					short color;

					if (bayerPattern[y, x - 1] == bayerPattern[y - 1, x])
					{
						color = CommonHelpers.GetMissingThirdColorIndex(bayerPattern[y, x - 1], bayerPattern[y - 1, x - 1]);
					}
					else
					{
						color = CommonHelpers.GetMissingThirdColorIndex(bayerPattern[y, x - 1], bayerPattern[y - 1, x]);
					}

					bayerPattern[y, x] = color;
				}
			}
		}
	}
}
