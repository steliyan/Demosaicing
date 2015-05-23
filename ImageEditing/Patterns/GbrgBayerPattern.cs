using System;
using ImageEditing.BaseClasses;
using ImageEditing.HelperClasses;

namespace ImageEditing.Patterns
{
	public class GbrgBayerPattern : BaseProperBayerPattern
	{
		public override string ToString()
		{
			return "GBRG";
		}

		public GbrgBayerPattern()
		{
			this.bayerPattern = BayerPatternConstants.Gbrg;
		}
	}
}
