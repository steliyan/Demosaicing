using System.Drawing;
using System.Drawing.Imaging;

namespace ImageEditing.BaseInterfaces
{
	public interface IFilter
	{
		Bitmap Apply(Image image);
		Bitmap Apply(Bitmap image);
		Bitmap Apply(BitmapData imageData);
	}
}
