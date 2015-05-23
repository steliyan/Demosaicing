using System;
using ImageEditing.BaseClasses;

namespace ImageEditing.HelperClasses
{
	static class BayerPatternConstants
	{
		#region Bayer patterns with 2 Red components

		public static readonly short[,] Grrb = new short[2, 2] {
			{ RGB.G, RGB.R },
			{ RGB.R, RGB.B }
		};

		public static readonly short[,] Rgbr = new short[2, 2] {
			{ RGB.R, RGB.G },
			{ RGB.B, RGB.R }
		};

		public static readonly short[,] Brrg = new short[2, 2] {
			{ RGB.B, RGB.R },
			{ RGB.R, RGB.G }
		};

		public static readonly short[,] Rbgr = new short[2, 2] {
			{ RGB.R, RGB.B },
			{ RGB.G, RGB.R }
		};

		public static readonly short[,] Grbr = new short[2, 2] {
			{ RGB.G, RGB.R },
			{ RGB.B, RGB.R }
		};

		public static readonly short[,] Brgr = new short[2, 2] {
			{ RGB.B, RGB.R },
			{ RGB.G, RGB.R }
		};

		public static readonly short[,] Rbrg = new short[2, 2] {
			{ RGB.R, RGB.B },
			{ RGB.R, RGB.G }
		};

		public static readonly short[,] Rgrb = new short[2, 2] {
			{ RGB.R, RGB.G },
			{ RGB.R, RGB.B }
		};

		public static readonly short[,] Rrgb = new short[2, 2] {
			{ RGB.R, RGB.R },
			{ RGB.G, RGB.B }
		};

		public static readonly short[,] Rrbg = new short[2, 2] {
			{ RGB.R, RGB.R },
			{ RGB.B, RGB.G }
		};

		public static readonly short[,] Gbrr = new short[2, 2] {
			{ RGB.G, RGB.B },	
			{ RGB.R, RGB.R }
		};

		public static readonly short[,] Bgrr = new short[2, 2] {
			{ RGB.B, RGB.G },
			{ RGB.R, RGB.R }
		};

		#endregion

		#region Bayer patterns with 2 Green components

		public static readonly short[,] Rggb = new short[2, 2] {
			{ RGB.R, RGB.G },
			{ RGB.G, RGB.B }
		};

		public static readonly short[,] Grbg = new short[2, 2] {
			{ RGB.G, RGB.R },
			{ RGB.B, RGB.G }
		};

		public static readonly short[,] Bggr = new short[2, 2] {
			{ RGB.B, RGB.G },
			{ RGB.G, RGB.R }
		};

		public static readonly short[,] Gbrg = new short[2, 2] {
			{ RGB.G, RGB.B },
			{ RGB.R, RGB.G }
		};

		public static readonly short[,] Rgbg = new short[2, 2] {
			{ RGB.R, RGB.G },
			{ RGB.B, RGB.G }
		};

		public static readonly short[,] Bgrg = new short[2, 2] {
			{ RGB.B, RGB.G },
			{ RGB.R, RGB.G }
		};

		public static readonly short[,] Gbgr = new short[2, 2] {
			{ RGB.G, RGB.B },
			{ RGB.G, RGB.R }
		};

		public static readonly short[,] Grgb = new short[2, 2] {
			{ RGB.G, RGB.R },
			{ RGB.G, RGB.B }
		};

		public static readonly short[,] Ggrb = new short[2, 2] {
			{ RGB.G, RGB.G },
			{ RGB.R, RGB.B }
		};

		public static readonly short[,] Ggbr = new short[2, 2] {
			{ RGB.G, RGB.G },
			{ RGB.B, RGB.R }
		};

		public static readonly short[,] Rbgg = new short[2, 2] {
			{ RGB.R, RGB.B },	
			{ RGB.G, RGB.G }
		};

		public static readonly short[,] Brgg = new short[2, 2] {
			{ RGB.B, RGB.R },
			{ RGB.G, RGB.G }
		};

		public static readonly short[,] Yamanaka = new short[2, 2] {
			{ RGB.G, RGB.R },
			{ RGB.G, RGB.B }
		};

		#endregion

		#region Bayer patterns with 2 Blue components

		public static readonly short[,] Rbbg = new short[2, 2] {
			{ RGB.R, RGB.B },
			{ RGB.B, RGB.G }
		};

		public static readonly short[,] Brgb = new short[2, 2] {
			{ RGB.B, RGB.R },
			{ RGB.G, RGB.B }
		};

		public static readonly short[,] Gbbr = new short[2, 2] {
			{ RGB.G, RGB.B },
			{ RGB.B, RGB.R }
		};

		public static readonly short[,] Bgrb = new short[2, 2] {
			{ RGB.B, RGB.G },
			{ RGB.R, RGB.B }
		};

		public static readonly short[,] Rbgb = new short[2, 2] {
			{ RGB.R, RGB.B },
			{ RGB.G, RGB.B }
		};

		public static readonly short[,] Gbrb = new short[2, 2] {
			{ RGB.G, RGB.B },
			{ RGB.R, RGB.B }
		};

		public static readonly short[,] Bgbr = new short[2, 2] {
			{ RGB.B, RGB.G },
			{ RGB.B, RGB.R }
		};

		public static readonly short[,] Brbg = new short[2, 2] {
			{ RGB.B, RGB.R },
			{ RGB.B, RGB.G }
		};

		public static readonly short[,] Bbrg = new short[2, 2] {
			{ RGB.B, RGB.B },
			{ RGB.R, RGB.G }
		};

		public static readonly short[,] Bbgr = new short[2, 2] {
			{ RGB.B, RGB.B },
			{ RGB.G, RGB.R }
		};

		public static readonly short[,] Rgbb = new short[2, 2] {
			{ RGB.R, RGB.G },	
			{ RGB.B, RGB.B }
		};

		public static readonly short[,] Grbb = new short[2, 2] {
			{ RGB.G, RGB.R },
			{ RGB.B, RGB.B }
		};

		#endregion

		#region Arrays containing bayer patterns

		public static readonly short[][,] BlueBayerPatterns =
			new short[][,] { Rbbg, Brgb, Gbbr, Bgrb, Rbgb, Gbrb, Bgbr, Brbg, Bbrg, Bbgr, Rgbb, Grbb };

		public static readonly short[][,] GreenBayerPatterns =
			new short[][,] { Rggb, Grbg, Bggr, Gbrg, Rgbg, Bgrg, Gbgr, Grgb, Ggrb, Ggbr, Rbgg, Brgg };

		public static readonly short[][,] RedBayerPatterns =
			new short[][,] { Grrb, Rgbr, Brrg, Rbgr, Grbr, Brgr, Rbrg, Rgrb, Rrgb, Rrbg, Gbrr, Bgrr };

		public static readonly short[][,] AllTwoColorBayerPatterns =
			new short[][,] {
				Rbbg, Brgb, Gbbr, Bgrb, Rbgb, Gbrb, Bgbr, Brbg, Bbrg, Bbgr, Rgbb, Grbb,
				Rggb, Grbg, Bggr, Gbrg, Rgbg, Bgrg, Gbgr, Grgb, Ggrb, Ggbr, Rbgg, Brgg,
				Grrb, Rgbr, Brrg, Rbgr, Grbr, Brgr, Rbrg, Rgrb, Rrgb, Rrbg, Gbrr, Bgrr
			};

		#endregion
	}
}
