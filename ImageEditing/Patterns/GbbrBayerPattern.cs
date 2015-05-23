using System;
using ImageEditing.BaseClasses;
using ImageEditing.HelperClasses;

namespace ImageEditing.Patterns
{
	public class GbbrBayerPattern : BaseProperBayerPattern
	{
		public override string ToString()
		{
			return "GBBR";
		}

		public GbbrBayerPattern()
		{
			this.bayerPattern = BayerPatternConstants.Gbbr;
		}
	}
}
