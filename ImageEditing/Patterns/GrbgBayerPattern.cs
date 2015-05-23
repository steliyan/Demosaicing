using System;
using ImageEditing.BaseClasses;
using ImageEditing.HelperClasses;

namespace ImageEditing.Patterns
{
	public class GrbgBayerPattern : BaseProperBayerPattern
	{
		public override string ToString()
		{
			return "GRBG";
		}

		public GrbgBayerPattern()
		{
			this.bayerPattern = BayerPatternConstants.Grbg;
		}
	}
}
