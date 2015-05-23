using System;
using ImageEditing.BaseInterfaces;

namespace ImageEditing.BaseClasses
{
	abstract public class BaseProperBayerPattern : IProperBayerPattern
	{
		protected short[,] bayerPattern;

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

		public virtual string BayerPatternShortName
		{
			get
			{
				return ToString();
			}
		}

		public abstract override string ToString();

		public BaseProperBayerPattern()
		{ }
	}
}
