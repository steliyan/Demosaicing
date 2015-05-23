using System;
using ImageEditing.BaseClasses;
using ImageEditing.HelperClasses;

namespace ImageEditing.Patterns
{
	public class BggrBayerPattern : BaseProperBayerPattern
	{
		public override string ToString()
		{
			return "BGGR";
		}

		public BggrBayerPattern()
		{
			this.bayerPattern = BayerPatternConstants.Bggr;
		}
	}
}
