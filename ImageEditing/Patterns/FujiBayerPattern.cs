using System;
using ImageEditing.BaseInterfaces;
using ImageEditing.HelperClasses;

namespace ImageEditing.Patterns
{
	public class FujiBayerPattern : IBayerPattern
	{
		private readonly short[,] bayerPattern;

		public short[,] BayerPattern
		{
			get { return bayerPattern; }
		}

		public bool IsBayerPatternGenerated
		{
			get { return true; }
		}

		public void GenerateBayerPattern()
		{ }

		public string BayerPatternShortName
		{
			get { return "FUJI"; }
		}

		public override string ToString()
		{
			return "Fuji";
		}

		public FujiBayerPattern()
		{
			this.bayerPattern = PatternConstants.FujiBayerPattern;
		}
	}
}
