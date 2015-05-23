using System;
using System.Drawing;

namespace ImageEditing.BaseClasses
{
	public class RGB
	{
		public const short R = 2;
		public const short G = 1;
		public const short B = 0;
		public const short A = 3;
		public static readonly short[] ColorChannels = { R, G, B };
		public static readonly short[] AlphaColorChannels = { A, R, G, B };

		public byte Red;
		public byte Green;
		public byte Blue;
		public byte Alpha;

		public Color Color
		{
			get { return Color.FromArgb(Alpha, Red, Green, Blue); }
			set
			{
				Red = value.R;
				Green = value.G;
				Blue = value.B;
				Alpha = value.A;
			}
		}

		public RGB()
		{
			Red = 0;
			Green = 0;
			Blue = 0;
			Alpha = 255;
		}

		public RGB(byte red, byte green, byte blue)
		{
			this.Red = red;
			this.Green = green;
			this.Blue = blue;
			this.Alpha = 255;
		}

		public RGB(byte red, byte green, byte blue, byte alpha)
		{
			this.Red = red;
			this.Green = green;
			this.Blue = blue;
			this.Alpha = alpha;
		}

		public RGB(Color color)
		{
			this.Red = color.R;
			this.Green = color.G;
			this.Blue = color.B;
			this.Alpha = color.A;
		}
	}
}
