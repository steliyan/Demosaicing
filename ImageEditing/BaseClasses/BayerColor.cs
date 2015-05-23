namespace ImageEditing.BaseClasses
{
	public struct BayerColor
	{
		public short ColorIndex;
		public byte ColorValue;

		public BayerColor(short colorIndex, byte colorValue)
		{
			ColorIndex = colorIndex;
			ColorValue = colorValue;
		}
	}
}
