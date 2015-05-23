using System;
using ImageEditing.BaseClasses;
using ImageEditing.HelperClasses;

namespace ImageEditing.Patterns
{
	public class RggbBayerPattern : BaseProperBayerPattern
	{
		public override string ToString()
		{
			return "RGGB";
		}

		public RggbBayerPattern()
		{
			this.bayerPattern = BayerPatternConstants.Rggb;
		}
	}
}
