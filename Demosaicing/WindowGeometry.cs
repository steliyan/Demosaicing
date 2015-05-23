using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ImageEditing.HelperClasses;

namespace Demosaicing
{
	public static class WindowGeometry
	{
		private const char SettingsDelimeter = '|';

		private static bool IsValidSize(Size size)
		{
			return CommonHelpers.IsInRange(size.Height, size.Width, Screen.PrimaryScreen.WorkingArea.Height, Screen.PrimaryScreen.WorkingArea.Width);
		}

		private static bool IsValidLocation(Point location, Size size)
		{
			return CommonHelpers.IsInRange(location.Y, location.X, location.Y + size.Height, location.X + size.Width);
		}

		public static string GeometryToString(Form form)
		{
			StringBuilder windowGeometry = new StringBuilder();
			windowGeometry.AppendFormat("{0}{1}", form.Location.X, SettingsDelimeter);
			windowGeometry.AppendFormat("{0}{1}", form.Location.Y, SettingsDelimeter);
			windowGeometry.AppendFormat("{0}{1}", form.Size.Width, SettingsDelimeter);
			windowGeometry.AppendFormat("{0}{1}", form.Size.Height, SettingsDelimeter);
			windowGeometry.AppendFormat("{0}", form.WindowState);

			return windowGeometry.ToString();
		}

		public static void GeometryFromString(string windowGeometry, Form form)
		{
			if (string.IsNullOrEmpty(windowGeometry))
			{
				return;
			}

			string[] numbers = windowGeometry.Split(SettingsDelimeter);
			string windowString = numbers[4];

			if (windowString == "Normal")
			{
				Point windowLocation = new Point(int.Parse(numbers[0]), int.Parse(numbers[1]));
				Size windowSize = new Size(int.Parse(numbers[2]), int.Parse(numbers[3]));

				bool isValidLocation = IsValidLocation(windowLocation, windowSize);
				bool isValidSize = IsValidSize(windowSize);

				if (isValidSize)
				{
					form.Size = windowSize;

					if (isValidLocation)
					{
						form.Location = windowLocation;
						form.StartPosition = FormStartPosition.Manual;
						form.WindowState = FormWindowState.Normal;
					}
				}
			}
			else if (windowString == "Maximized")
			{
				form.Location = new Point(25, 25);
				form.StartPosition = FormStartPosition.Manual;
				form.WindowState = FormWindowState.Maximized;
			}
		}
	}
}
