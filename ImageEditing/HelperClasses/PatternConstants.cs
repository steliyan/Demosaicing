using System;
using ImageEditing.BaseClasses;

namespace ImageEditing.HelperClasses
{
	public static class PatternConstants
	{
		public static readonly short[,] DefaultBayerPattern = new short[2, 2] {
			{ RGB.G, RGB.R },
			{ RGB.B, RGB.G }
		};

		public static readonly short[,] FujiBayerPattern = new short[6, 6] {
			{ RGB.G, RGB.B, RGB.G, RGB.G, RGB.R, RGB.G },
			{ RGB.R, RGB.G, RGB.R, RGB.B, RGB.G, RGB.B },
			{ RGB.G, RGB.B, RGB.G, RGB.G, RGB.R, RGB.G },
			{ RGB.G, RGB.R, RGB.G, RGB.G, RGB.B, RGB.G },
			{ RGB.B, RGB.G, RGB.B, RGB.R, RGB.G, RGB.R },
			{ RGB.G, RGB.R, RGB.G, RGB.G, RGB.B, RGB.G }
		};

		public static readonly short[,] SobelPatternX3 = new short[3, 3]
		{
			{ -1, -1, -1},
			{ 0, 0, 0 },
			{ 1, 1, 1},
		};

		public static readonly short[,] SobelPatternY3 = new short[3, 3]
		{
			{ -1, 0, 1},
			{ -1, 0, 1},
			{ -1, 0, 1}
		};

		public static readonly short[,] SobelPatternX5 = new short[3, 5]
		{
			{ -1, -1, -1, -1, -1},
			{ 0, 0, 0, 0, 0 },
			{ 1, 1, 1, 1, 1},
		};

		public static readonly short[,] SobelPatternY5 = new short[5, 3]
		{
			{ -1, 0, 1},
			{ -1, 0, 1},
			{ -1, 0, 1},
			{ -1, 0, 1},
			{ -1, 0, 1}
		};

		public static readonly short[,] SobelPatternX7 = new short[3, 7]
		{
			{ -1, -1, -1, -1, -1, -1, -1 },
			{ 0, 0, 0, 0, 0, 0, 0 },
			{ 1, 1, 1, 1, 1, 1, 1 },
		};

		public static readonly short[,] SobelPatternY7 = new short[7, 3]
		{
			{ -1, 0, 1},
			{ -1, 0, 1},
			{ -1, 0, 1},
			{ -1, 0, 1},
			{ -1, 0, 1},
			{ -1, 0, 1},
			{ -1, 0, 1}
		};

		public static readonly short[,] GreenKernel3X3 = new short[3, 3]
		{
			{0, 1, 0},
			{1, 4, 1},
			{0, 1, 0}
		};

		public static readonly short[,] RedBlueKernel3X3 = new short[3, 3]
		{
			{1, 2, 1},
			{2, 4, 2},
			{1, 2, 1}
		};

		public static readonly short[,] GreenKernel7X7 = new short[7, 7]
		{
			{0, 0, 0, 1, 0, 0, 0},
			{0, 0, -9, 0, -9, 0, 0},
			{0, -9, 0, 81, 0, -9, 0},
			{1, 0, 81, 256, 81, 0, 1},
			{0, -9, 0, 81, 0, -9, 0},
			{0, 0, -9, 0, -9, 0, 0},
			{0, 0, 0, 1, 0, 0, 0}
		};

		public static readonly short[,] RedBlueKernel7X7 = new short[7, 7]
		{
			{1, 0, -9, -16, -9, 0, 1},
			{0, 0, 0, 0, 0, 0, 0},
			{-9, 0, 81, 144, 81, 0, -9},
			{-16, 0, 144, 256, 144, 0, -16},
			{-9, 0, 81, 144, 81, 0, -9},
			{0, 0, 0, 0, 0, 0, 0},
			{1, 0, -9,-16, -9, 0, 1}
		};
	}
}
