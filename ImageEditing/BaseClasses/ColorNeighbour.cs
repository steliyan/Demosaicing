using System;
using System.Linq;

namespace ImageEditing.BaseClasses
{
	public struct ColorNeighbour
	{
		public readonly short Color;
		public readonly short[] ColorBlock;

		public ColorNeighbour(short color, short[] colorBlock)
		{
			this.Color = color;
			this.ColorBlock = colorBlock;
		}

		public override bool Equals(object obj)
		{
			if (obj is ColorNeighbour)
			{
				ColorNeighbour other = (ColorNeighbour)obj;

				bool areEqual =
					this.Color.Equals(other.Color) &&
					this.ColorBlock.SequenceEqual(other.ColorBlock);

				return areEqual;
			}

			return false;
		}
	}
}
