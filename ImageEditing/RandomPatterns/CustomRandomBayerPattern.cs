using System;
using System.Collections.Generic;
using System.Linq;
using ImageEditing.BaseClasses;
using ImageEditing.BaseInterfaces;
using ImageEditing.HelperClasses;

namespace ImageEditing.RandomPatterns
{
	// TODO: Fix adjacencies for custom bayer random generator
	public class CustomRandomBayerPattern : IRandomBayerPattern
	{
		private static readonly Random rand = new Random();

		// bayer pattern
		private bool isPatternGenerated = false;
		private readonly bool isSizeDefined = false;
		private readonly int width;
		private readonly int height;
		private readonly short[,] bayerPattern;

		// color blocks
		private const int ColorBlockLength = 2;
		private readonly short[] rg = { RGB.R, RGB.G };
		private readonly short[] rb = { RGB.R, RGB.B };
		private readonly short[] gr = { RGB.G, RGB.R };
		private readonly short[] gb = { RGB.G, RGB.B };
		private readonly short[] br = { RGB.B, RGB.R };
		private readonly short[] bg = { RGB.B, RGB.G };
		private readonly Dictionary<short[], HashSet<short[]>> rowColumnAdjacency = new Dictionary<short[], HashSet<short[]>>();
		private readonly Dictionary<short[], HashSet<short[]>> blocksAdjacency = new Dictionary<short[], HashSet<short[]>>();
		private readonly Dictionary<ColorNeighbour, HashSet<short[]>> neighbourBlocksAdjacency = new Dictionary<ColorNeighbour, HashSet<short[]>>();

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
				InitAdjacencyRules();
				FillFirstRow();
				FillFirstColumn();
				FillBayerPattern();
				FillLastColumn();
				CorrectMissingColors();
				isPatternGenerated = true;
			}
		}

		public CustomRandomBayerPattern()
		{ }

		public CustomRandomBayerPattern(int width, int height)
		{
			this.isSizeDefined = true;
			this.width = width;
			this.height = height;
			this.bayerPattern = new short[height, width];
		}

		public string BayerPatternShortName
		{
			get { return "CUSTOM"; }
		}

		public override string ToString()
		{
			return "Custom Random Bayer";
		}

		private void InitRowColumnAdjacencyRules()
		{
			HashSet<short[]> adjRg = new HashSet<short[]>();
			adjRg.Add(rg);
			adjRg.Add(rb);
			adjRg.Add(br);
			adjRg.Add(bg);
			rowColumnAdjacency.Add(rg, adjRg);

			HashSet<short[]> adjRb = new HashSet<short[]>();
			adjRb.Add(rg);
			adjRb.Add(rb);
			adjRb.Add(gr);
			adjRb.Add(gb);
			rowColumnAdjacency.Add(rb, adjRb);

			HashSet<short[]> adjGr = new HashSet<short[]>();
			adjGr.Add(gr);
			adjGr.Add(gb);
			adjGr.Add(br);
			adjGr.Add(bg);
			rowColumnAdjacency.Add(gr, adjGr);

			HashSet<short[]> adjGb = new HashSet<short[]>();
			adjGb.Add(gr);
			adjGb.Add(gb);
			adjGb.Add(rg);
			adjGb.Add(rb);
			rowColumnAdjacency.Add(gb, adjGb);

			HashSet<short[]> adjBr = new HashSet<short[]>();
			adjBr.Add(br);
			adjBr.Add(bg);
			adjBr.Add(gr);
			adjBr.Add(gb);
			rowColumnAdjacency.Add(br, adjBr);

			HashSet<short[]> adjBg = new HashSet<short[]>();
			adjBg.Add(br);
			adjBg.Add(bg);
			adjBg.Add(rg);
			adjBg.Add(rb);
			rowColumnAdjacency.Add(bg, adjBg);
		}

		private void InitBlocksAdjacencyRules()
		{
			HashSet<short[]> adjRg = new HashSet<short[]>();
			adjRg.Add(gr);
			adjRg.Add(gb);
			adjRg.Add(br);
			blocksAdjacency.Add(rg, adjRg);

			HashSet<short[]> adjRb = new HashSet<short[]>();
			adjRb.Add(gr);
			adjRb.Add(br);
			adjRb.Add(bg);
			blocksAdjacency.Add(rb, adjRb);

			HashSet<short[]> adjGr = new HashSet<short[]>();
			adjGr.Add(rg);
			adjGr.Add(rb);
			adjGr.Add(bg);
			blocksAdjacency.Add(gr, adjGr);

			HashSet<short[]> adjGb = new HashSet<short[]>();
			adjGb.Add(rg);
			adjGb.Add(br);
			adjGb.Add(bg);
			blocksAdjacency.Add(gb, adjGb);

			HashSet<short[]> adjBr = new HashSet<short[]>();
			adjBr.Add(rg);
			adjBr.Add(rb);
			adjBr.Add(gb);
			blocksAdjacency.Add(br, adjBr);

			HashSet<short[]> adjBg = new HashSet<short[]>();
			adjBg.Add(rb);
			adjBg.Add(gr);
			adjBg.Add(gb);
			blocksAdjacency.Add(bg, adjBg);
		}

		private void InitNeighbourBlocksAdjacencyRules()
		{
			short[] colors = { RGB.R, RGB.G, RGB.B };

			foreach (var color in colors)
			{
				foreach (var colorBlock in blocksAdjacency.Keys)
				{
					// init our tuple
					var tuple = new ColorNeighbour(color, colorBlock);

					// get all possible color blocks, which don't overlap with top/left neighbour colors
					var possibleColorBlocks = blocksAdjacency[colorBlock].Select(cb => cb).Where(cb => cb[0] != color);

					// add possible color blocks to hashset
					HashSet<short[]> adjacencyBlocks = new HashSet<short[]>();
					foreach (var possibleColorBlock in possibleColorBlocks)
					{
						adjacencyBlocks.Add(possibleColorBlock);
					}

					// add tuple and hashset to our final dictionary
					neighbourBlocksAdjacency.Add(tuple, adjacencyBlocks);
				}
			}
		}

		private void InitAdjacencyRules()
		{
			this.InitRowColumnAdjacencyRules();
			this.InitBlocksAdjacencyRules();
			this.InitNeighbourBlocksAdjacencyRules();
		}

		private void FillFirstRow()
		{
			// calculate the needed initial blocks
			int blocksCount = (int)Math.Ceiling((double)width / ColorBlockLength);
			List<short[]> blocks = new List<short[]>();

			// initially "rg" block
			int startRandomIndex = rand.Next(0, rowColumnAdjacency.Keys.Count);
			short[] startBlock = rowColumnAdjacency.ElementAt(startRandomIndex).Key;
			blocks.Add(startBlock);

			// fill the dummy list
			for (int i = 1; i < blocksCount; i++)
			{
				short[] prevBlock = blocks[i - 1];
				int randomIndex = rand.Next(0, rowColumnAdjacency[prevBlock].Count);
				short[] nextBlock = rowColumnAdjacency[prevBlock].ElementAt(randomIndex);
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
			var possibleBlocks = rowColumnAdjacency.Keys.Select(k => k).Where(k => k[0] != bayerPattern[0, 0]).ToList();
			int startRandomIndex = rand.Next(0, possibleBlocks.Count);
			short[] startBlock = possibleBlocks.ElementAt(startRandomIndex);
			blocks.Add(startBlock);

			// fill the dummy list
			for (int i = 1; i < blocksCount; i++)
			{
				short[] prevBlock = blocks[i - 1];
				int randomIndex = rand.Next(0, rowColumnAdjacency[prevBlock].Count);
				short[] nextBlock = rowColumnAdjacency[prevBlock].ElementAt(randomIndex);
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
					bayerPattern[y, x] = -1;
				}
			}

			for (int y = 1; y < height; y++)
			{
				for (int x = 1; x < width - 1; x += 2)
				{
					short left = bayerPattern[y, x - 1];
					short top = bayerPattern[y - 1, x];
					short topRight = bayerPattern[y - 1, x + 1];

					// TODO: Add check for out of bounds
					short prevColor = left;
					short[] prevColorBlock = { top, topRight };

					var tuple = new ColorNeighbour(prevColor, prevColorBlock);
					var possibleColorBlocks = neighbourBlocksAdjacency[tuple];

					int randomIndex = rand.Next(0, possibleColorBlocks.Count);
					var randomColorBlock = possibleColorBlocks.ElementAt(randomIndex);

					bayerPattern[y, x] = randomColorBlock[0];
					bayerPattern[y, x + 1] = randomColorBlock[1];
				}
			}
		}

		private void FillLastColumn()
		{
			// get last columnt index
			int x = width - 1;

			// we filed odd number of columns (becaouse of first column)
			if (x % 2 == 1)
			{
				for (int y = 1; y < height; y++)
				{
					short topColor = bayerPattern[y - 1, x];
					short leftColor = bayerPattern[y, x - 1];
					short topLeftColor = bayerPattern[y - 1, x - 1];

					if (topColor == leftColor)
					{
						bayerPattern[y, x] = topLeftColor;
					}
					else
					{
						bayerPattern[y, x] = CommonHelpers.GetMissingThirdColorIndex(topColor, leftColor);
					}
				}
			}
		}

		private void CorrectMissingColors()
		{
			for (int y = 0; y < height - 1; y += 2)
			{
				for (int x = 0; x < width - 1; x += 2)
				{
					bool hasRColor = bayerPattern[y, x] == RGB.R || bayerPattern[y + 1, x] == RGB.R || bayerPattern[y, x + 1] == RGB.R || bayerPattern[y + 1, x + 1] == RGB.R;
					bool hasGColor = bayerPattern[y, x] == RGB.G || bayerPattern[y + 1, x] == RGB.G || bayerPattern[y, x + 1] == RGB.G || bayerPattern[y + 1, x + 1] == RGB.G;
					bool hasBColor = bayerPattern[y, x] == RGB.B || bayerPattern[y + 1, x] == RGB.B || bayerPattern[y, x + 1] == RGB.B || bayerPattern[y + 1, x + 1] == RGB.B;

					if (!hasRColor)
					{
						bayerPattern[y + 1, x + 1] = RGB.R;
					}

					if (!hasGColor)
					{
						bayerPattern[y + 1, x + 1] = RGB.G;
					}

					if (!hasBColor)
					{
						bayerPattern[y + 1, x + 1] = RGB.B;
					}
				}
			}
		}
	}
}
