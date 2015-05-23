namespace ImageEditing.BaseInterfaces
{
	public interface IBayerPattern
	{
		string BayerPatternShortName { get; }
		short[,] BayerPattern { get; }
		bool IsBayerPatternGenerated { get; }
		void GenerateBayerPattern();
	}
}
