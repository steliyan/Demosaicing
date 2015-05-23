using System;
using ImageEditing.BaseClasses;
using ImageEditing.HelperClasses;

namespace ImageEditing.Patterns
{
	public class GrrbBayerPattern : BaseProperBayerPattern
	{
		public override string ToString()
		{
			return "GRRB";
		}

		public GrrbBayerPattern()
		{
			this.bayerPattern = BayerPatternConstants.Grrb;
		}
	}
}
