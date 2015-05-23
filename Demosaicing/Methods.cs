using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ImageEditing;
using ImageEditing.BaseInterfaces;
using ImageEditing.Exceptions;
using ImageEditing.Filters;
using ImageEditing.HelperClasses;
using ImageEditing.Patterns;

namespace Demosaicing
{
	public partial class MainForm
	{
		// TODO: Exception is thrown, when loading big image!!
		private void LoadImage(Image image, PictureBox pictureBox, ref Bitmap bitmap)
		{
			if (image != null)
			{
				int maxWidth = pictureBox.Width - (pictureBox.Padding.Left + pictureBox.Padding.Right);
				int maxHeight = pictureBox.Height - (pictureBox.Padding.Top + pictureBox.Padding.Bottom);

				if (maxWidth <= 0 || maxHeight <= 0)
				{
					return;
				}

				// force freeing of allocated resources
				if (pictureBox.Image != null)
				{
					pictureBox.Image.Dispose();
				}

				Image imageToLoad = BitmapHelpers.ResizeImage(image, new Size(maxWidth, maxHeight));
				pictureBox.Image = imageToLoad;

				// force freeing of allocated resources
				if (bitmap != null)
				{
					bitmap.Dispose();
				}

				// load picturebox to bitmap
				bitmap = new Bitmap(imageToLoad);
				imageWidth = pictureBoxLeft.Image.Width;
				imageHeight = pictureBoxLeft.Image.Height;
				xMouseOffset = (pictureBoxWidth - imageWidth) / 2;
				yMouseOffset = (pictureBoxHeight - imageHeight) / 2;
			}
		}

		private void LoadImages(bool forceUpdate = false)
		{
			try
			{
				image = Image.FromFile(imagePath);
				LoadImage(image, pictureBoxLeft, ref bitmapLeft);

				UpdateFilters(forceUpdate);
				ApplyAllFilters();
				labelStatus.Text = string.Format("Loaded: \"{0}\"", Path.GetFileName(imagePath));
			}
			catch (Exception ex)
			{
				imagePath = validImagePath;
				labelStatus.Text =
					"Couldn't load original image!" + Environment.NewLine +
					ErrorString + ex.Message;
			}
		}

		private void RotateImage(Image image, RotateFlipType rotateFlipType, PictureBox pictureBox, ref Bitmap bitmap)
		{
			image.RotateFlip(rotateFlipType);
			LoadImage(image, pictureBox, ref bitmap);
		}

		private void RotateImages(RotateFlipType rotateFlipType)
		{
			RotateImage(image, rotateFlipType, pictureBoxLeft, ref bitmapLeft);
			RotateImage(filteredImage, rotateFlipType, pictureBoxRight, ref bitmapRight);
		}

		private void ApplyGrayscaleBayerFilter(bool loadImage = true)
		{
			if (image != null)
			{
				try
				{
					filteredImage = grayscaleBayerFilter.Apply(image);

					if (loadImage)
					{
						LoadImage(filteredImage, pictureBoxRight, ref bitmapRight);
					}

					labelStatus.Text = string.Format("Applied grayscale bayer filter to: \"{0}\"", Path.GetFileName(imagePath));
				}
				catch (InvalidPixelFormatException ex)
				{
					labelStatus.Text =
						"Couldn't apply GrayscaleBayer filter!" + Environment.NewLine +
						ErrorString + ex.Message;
				}
			}
		}

		private void ApplyDemosaicingFilter(bool loadImage = true)
		{
			if (filteredImage != null)
			{
				try
				{
					filteredImage = demosaicingFilter.Apply(filteredImage);

					if (loadImage)
					{
						LoadImage(filteredImage, pictureBoxRight, ref bitmapRight);
					}

					fileNameSuffix = string.Format(
						"_{0}_{1}",
						bayerPattern.BayerPatternShortName,
						(demosaicingFilter as IDemosaicer).DemosaicerShortname);
					labelStatus.Text = string.Format("Demosaiced image: \"{0}\"", Path.GetFileName(imagePath));
				}
				catch (InvalidImageException ex)
				{
					labelStatus.Text =
						"Image is already demosaiced!" + Environment.NewLine +
						ErrorString + ex.Message;
				}
				catch (InvalidPixelFormatException ex)
				{
					labelStatus.Text =
						"Couldn't apply Demosaicing filter!" + Environment.NewLine +
						ErrorString + ex.Message;
				}
			}
		}

		private void ApplyColorChannelsFilter(bool loadImage = true)
		{
			if (filteredImage != null)
			{
				try
				{
					filteredImage = colorChannelsFilter.Apply(filteredImage);

					if (loadImage)
					{
						LoadImage(filteredImage, pictureBoxRight, ref bitmapRight);
					}
				}
				catch (Exception ex)
				{
					labelStatus.Text =
						"Error occurred processing the filtered image!" + Environment.NewLine +
						ErrorString + ex.Message;
				}
			}
		}

		private void ApplyBayerFilter(bool loadImage = true)
		{
			if (filteredImage != null)
			{
				try
				{
					filteredImage = bayerFilter.Apply(filteredImage);

					if (loadImage)
					{
						LoadImage(filteredImage, pictureBoxRight, ref bitmapRight);
					}

					labelStatus.Text = string.Format("Applied bayer filter to: \"{0}\"", Path.GetFileName(imagePath));
				}
				catch (InvalidPixelFormatException ex)
				{
					labelStatus.Text =
						"Couldn't apply Bayer filter!" + Environment.NewLine +
						ErrorString + ex.Message;
				}
			}
		}

		private void InitRandomBayerFilters(bool forceUpdate = false)
		{
			IRandomBayerPattern randomBayerPattern = bayerPattern as IRandomBayerPattern;
			if (randomBayerPattern != null && (forceUpdate || !randomBayerPattern.IsSizeDefined))
			{
				object obj = Activator.CreateInstance(bayerPattern.GetType(), image.Width, image.Height);
				bayerPattern = obj as IRandomBayerPattern;
			}
		}

		private void UpdateFilters(bool forceUpdate = false)
		{
			colorChannelsFilter = new ExtractColorChannelsFilter(colorChannels);
			InitRandomBayerFilters(forceUpdate);
			bayerPattern.GenerateBayerPattern();

			// TODO: Add GrayscaleBayerFilter and BayerFilter to IBayerPattern interface!!!
			if (bayerPattern is IProperBayerPattern)
			{
				grayscaleBayerFilter = new GrayscaleBayerFilter(bayerPattern.BayerPattern);
				bayerFilter = new BayerFilter(bayerPattern.BayerPattern);
			}
			else if (bayerPattern is IRandomBayerPattern)
			{
				grayscaleBayerFilter = new GrayscaleRandomBayerFilter(bayerPattern.BayerPattern);
				bayerFilter = new RandomBayerFilter(bayerPattern.BayerPattern);
			}
			else if (bayerPattern is FujiBayerPattern)
			{
				grayscaleBayerFilter = new GrayscaleBayer6X6Filter();
				bayerFilter = new Bayer6X6Filter();
			}
		}
		
		private void ApplyAllFilters()
		{
			ApplyGrayscaleBayerFilter(false);
			ApplyBayerFilter(false);
			ApplyDemosaicingFilter(false);
			ApplyColorChannelsFilter(true);
		}

		private void ShowImageMetrics()
		{
			try
			{
				ImageMetrics imageMetrics = new ImageMetrics(image, filteredImage);

				string metrics =
					imageMetrics.MSEFriendlyName + Environment.NewLine +
					imageMetrics.PSNRFriendlyName;
				labelStatus.Text = metrics;
			}
			catch (InvalidImageException ex)
			{
				labelStatus.Text =
					"Incompatible images!" + Environment.NewLine +
					ErrorString + ex.Message;
			}
		}

		// TODO: Fix showing of tooltip, when image is closer to top/bottom edge!
		private void ShowColorTooltip(Control control, MouseEventArgs e)
		{
			if (
				bitmapLeft == null ||
				bitmapRight == null ||
				bitmapLeft.Size != bitmapRight.Size)
			{
				return;
			}

			int x = Math.Min(Math.Max(0, e.X - xMouseOffset), bitmapLeft.Width - xMouseOffset);
			int y = Math.Min(Math.Max(0, e.Y - yMouseOffset), bitmapLeft.Height - yMouseOffset);

			try
			{
				Color color1 = bitmapLeft.GetPixel(x, y);
				Color color2 = bitmapRight.GetPixel(x, y);

				string toolTipText =
					string.Format(ColorCoordsFormatString, x, y) + Environment.NewLine +
					string.Format(ColorFormatString, "Left", color1.R, color1.G, color1.B) + Environment.NewLine +
					string.Format(ColorFormatString, "Right", color2.R, color2.G, color2.B);

				toolTipColor.Show(toolTipText, control);
			}
			catch (Exception)
			{ }
		}

		private void UpdateMainFormControls()
		{
			// reload images with new sizes
			LoadImage(image, pictureBoxLeft, ref bitmapLeft);
			LoadImage(filteredImage, pictureBoxRight, ref bitmapRight);

			// get pictureboxes' new sizes
			pictureBoxWidth = pictureBoxLeft.Width;
			pictureBoxHeight = pictureBoxLeft.Height;

			// resize label width
			labelStatus.Width =
				groupBoxButtons.Right - labelStatus.Left -
				(groupBoxButtons.Margin.Right + groupBoxButtons.Padding.Right + labelStatus.Margin.Right + labelStatus.Padding.Right);
		}

		private void BrowseForFile()
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.InitialDirectory = Path.GetFullPath(InitialDirectory);
				openFileDialog.Filter = FileFilter;
				openFileDialog.FilterIndex = FilterIndex;
				openFileDialog.RestoreDirectory = RestoreDirectory;

				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					validImagePath = imagePath;

					try
					{
						imagePath = openFileDialog.FileName;
						LoadImages(true);
					}
					catch (Exception ex)
					{
						imagePath = validImagePath;
						labelStatus.Text =
							"Could not read file from disk!" + Environment.NewLine +
							ErrorString + ex.Message;
					}
				}
			}
		}

		private string GetFileExtensionFromImage(Image image)
		{
			if (image != null)
			{
				if (ImageFormat.Bmp.Equals(image.RawFormat))
				{
					return "*.BMP";
				}
				else if (ImageFormat.Jpeg.Equals(image.RawFormat))
				{
					return "*.JPG";
				}
				else if (ImageFormat.Gif.Equals(image.RawFormat))
				{
					return "*.GIF";
				}
				else if (ImageFormat.Tiff.Equals(image.RawFormat))
				{
					return "*.TIF";
				}
				else if (ImageFormat.Png.Equals(image.RawFormat))
				{
					return "*.PNG";
				}
			}

			return string.Empty;
		}

		private void SaveFilteredImageToFile()
		{
			using (SaveFileDialog saveFileDialog = new SaveFileDialog())
			{
				string fileFilter = string.Format(FilterFormatString, GetFileExtensionFromImage(image));

				string fullPath = Path.GetFullPath(imagePath);
				string fileName = Path.GetFileNameWithoutExtension(fullPath) + fileNameSuffix;
				string directory = Path.GetDirectoryName(fullPath);

				saveFileDialog.InitialDirectory = directory;
				saveFileDialog.FileName = fileName;
				saveFileDialog.Filter = fileFilter;
				saveFileDialog.RestoreDirectory = RestoreDirectory;

				if (saveFileDialog.ShowDialog() == DialogResult.OK)
				{
					try
					{
						filteredImage.Save(saveFileDialog.FileName, image.RawFormat);
						labelStatus.Text = string.Format("Successfully wrote image: {0}", saveFileDialog.FileName);
					}
					catch (Exception ex)
					{
						labelStatus.Text =
							"Could not write image to disk!" + Environment.NewLine +
							ErrorString + ex.Message;
					}
				}
			}
		}

		private void ShowAboutBox()
		{
			using (AboutBox aboutBox = new AboutBox())
			{
				aboutBox.ShowDialog();
			}
		}

		private void RedrawBorder(PaintEventArgs e)
		{
			if (e != null)
			{
				Graphics graphics = e.Graphics;
				Pen pen = new Pen(Color.DimGray, 1);

				Point topLeft = new Point(0, 5);
				Point topRight = new Point(0, e.ClipRectangle.Height - 2);
				Point bottomLeft = new Point(e.ClipRectangle.Width - 2, 5);
				Point bottomRight = new Point(e.ClipRectangle.Width - 2, e.ClipRectangle.Height - 2);

				graphics.DrawLine(pen, topLeft, topRight);
				graphics.DrawLine(pen, topRight, bottomRight);
				graphics.DrawLine(pen, bottomRight, bottomLeft);
				graphics.DrawLine(pen, bottomLeft, topLeft);
			}
		}

		private void AddColorChannel(short colorChannel)
		{
			if (!colorChannels.Contains(colorChannel))
			{
				colorChannels.Add(colorChannel);
			}
		}

		private void RemoveColorChannel(short colorChannel)
		{
			if (colorChannels.Contains(colorChannel))
			{
				colorChannels.Remove(colorChannel);
			}
		}
	}
}
