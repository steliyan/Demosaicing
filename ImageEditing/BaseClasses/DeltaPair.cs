using System;

namespace ImageEditing.BaseClasses
{
	public struct DeltaPair
	{
		public int DeltaH1;
		public int DeltaH2;
		public int DeltaV1;
		public int DeltaV2;

		public DeltaPair(int deltaH1, int deltaH2, int deltaV1, int deltaV2)
		{
			DeltaH1 = deltaH1;
			DeltaH2 = deltaH2;
			DeltaV1 = deltaV1;
			DeltaV2 = deltaV2;
		}
	}
}
