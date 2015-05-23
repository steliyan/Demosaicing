using System;
using ImageEditing.BaseClasses;
using ImageEditing.HelperClasses;

namespace ImageEditing.Patterns
{
	public class YamanakaBayerPattern : BaseProperBayerPattern
	{
		public override string ToString()
		{
			return "Yamanaka";
		}

		public YamanakaBayerPattern()
		{
			this.bayerPattern = BayerPatternConstants.Yamanaka;
		}
	}
}
