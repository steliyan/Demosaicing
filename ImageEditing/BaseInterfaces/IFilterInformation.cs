using System.Collections.Generic;
using System.Drawing.Imaging;

namespace ImageEditing.BaseInterfaces
{
	public interface IFilterInformation
	{
		Dictionary<PixelFormat, PixelFormat> FormatTranslations { get; }
	}
}
