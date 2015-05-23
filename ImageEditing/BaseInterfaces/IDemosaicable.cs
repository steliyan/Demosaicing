using System.Collections.Generic;

namespace ImageEditing.BaseInterfaces
{
	public interface IDemosaicable
	{
		List<IDemosaicer> Demosaicers { get; }
	}
}
