using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ImageEditing.BaseClasses;
using ImageEditing.BaseInterfaces;
using ImageEditing.Filters;
using ImageEditing.HelperClasses;
using ImageEditing.Patterns;
using ImageEditing.RandomPatterns;

namespace Demosaicing
{
	public partial class MainForm : Form
	{
		#region Global vars (internal state, etc)

		// Common string
		private const string ErrorString = "Error: ";
		private const string ColorCoordsFormatString = "Colors at ({0}, {1}): ";
		private const string ColorFormatString = @"  {{{0}: R={1}, G={2}, B={3}}}";
		private const string FilterFormatString = @"Image File({0})|{0}|All files (*.*)|*.*";

		// Open/SaveFileDialog properties
		private const string InitialFile = @"IM001.tif";
		private const string InitialDirectory = @"Resources\Images\Color\";
		private const string FileFilter = @"Image Files (*.BMP;*.JPG;*.GIF;*.TIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.TIF;*.PNG|All files (*.*)|*.*";
		private const int FilterIndex = 1;
		private const bool RestoreDirectory = true;

		// Color filtering
		private List<short> colorChannels = new List<short>(new short[] { RGB.R, RGB.G, RGB.B });
		private ExtractColorChannelsFilter colorChannelsFilter = new ExtractColorChannelsFilter(new short[] { RGB.R, RGB.G, RGB.B });

		// Zoom factors
		private bool isZoomed = false;
		private readonly int[] zoomFactors = new int[] { 2, 3, 5, 10 };

		// Bayer pattern holders
		private IBayerPattern bayerPattern;
		private readonly IBayerPattern[] bayerPatterns =
			{
				new BggrBayerPattern(),
				new GbrgBayerPattern(),
				new GrbgBayerPattern(),
				new RggbBayerPattern(),
				new GbbrBayerPattern(),
				new GrrbBayerPattern(),
				new YamanakaBayerPattern(),
				new FujiBayerPattern(),
				new CustomRandomBayerPattern(),
				new TilingRandomBayerPattern(),
				new TwoColorRandomBayerRandom(),
				new NaiveRandomBayerPattern()
			};

		// Concrete vars
		private string imagePath = Path.Combine(InitialDirectory, InitialFile);
		private string validImagePath;
		private Image image;
		private Image colorExtractedImage;
		private Image filteredImage;
		private Image colorExtractedFilteredImage;
		private IFilter bayerFilter;
		private IFilter grayscaleBayerFilter;
		private IFilter demosaicingFilter;

		// Cache variables
		private int imageWidth;
		private int imageHeight;
		private int pictureBoxWidth;
		private int pictureBoxHeight;
		private int xMouseOffset;
		private int yMouseOffset;
		private Bitmap bitmapLeft;
		private Bitmap bitmapRight;
		private string fileNameSuffix;

		#endregion

		#region MainForm methods/events

		public MainForm()
		{
			// form init
			this.InitializeComponent();
			this.MinimumSize = this.Size;

			// bind to bayer patterns
			this.comboBoxBayerPattern.DataSource = this.bayerPatterns;
			this.comboBoxScaleFactor.DataSource = this.zoomFactors;

			// read last window settings
			WindowGeometry.GeometryFromString(Properties.Settings.Default.WindowGeometry, this);
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			LoadImages();
		}

		private void MainForm_Resize(object sender, EventArgs e)
		{
			UpdateMainFormControls();
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			// write current window settings
			var geometry = WindowGeometry.GeometryToString(this);
			Properties.Settings.Default.WindowGeometry = geometry;
			Properties.Settings.Default.Save();
		}

		#endregion

		#region MainForm buttons' events

		private void ButtonBrowseClick(object sender, EventArgs e)
		{
			BrowseForFile();
		}

		private void ButtonSaveClick(object sender, EventArgs e)
		{
			SaveFilteredImageToFile();
		}

		private void ButtonGrayscaleClick(object sender, EventArgs e)
		{
			ApplyGrayscaleBayerFilter(true);
		}

		private void ButtonBayerClick(object sender, EventArgs e)
		{
			ApplyGrayscaleBayerFilter(false);
			ApplyBayerFilter(true);
		}

		private void ButtonDemosaicClick(object sender, EventArgs e)
		{
			ApplyGrayscaleBayerFilter(false);
			ApplyBayerFilter(false);
			ApplyDemosaicingFilter(true);
		}

		private void ButtonPsnrClick(object sender, EventArgs e)
		{
			ShowImageMetrics();
		}

		private void ButtonAboutClick(object sender, EventArgs e)
		{
			ShowAboutBox();
		}

		#endregion

		#region MainForm controls' events

		private void ComboBoxBayerPatternSelectedValueChanged(object sender, EventArgs e)
		{
			bayerPattern = (IBayerPattern)comboBoxBayerPattern.SelectedItem;

			UpdateFilters();

			// bind to demoisacers
			comboBoxDemosaicer.DataSource = (bayerFilter as IDemosaicable).Demosaicers;
		}

		private void ComboBoxDemosaicerSelectedValueChanged(object sender, EventArgs e)
		{
			demosaicingFilter = comboBoxDemosaicer.SelectedValue as IFilter;

			// we need to handle the specific bayer pattern
			if (demosaicingFilter != null && demosaicingFilter is MhcDemosaicing)
			{
				demosaicingFilter = new MhcDemosaicing(bayerPattern.BayerPattern);
			}
		}

		private void LabelStatusTextChanged(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(labelStatus.Text))
			{
				if (labelStatus.Text.Contains(ErrorString))
				{
					labelStatus.ForeColor = Color.Red;
					pictureBoxStatus.Image = Properties.Resources.Error;
				}
				else
				{
					labelStatus.ForeColor = Color.Black;
					pictureBoxStatus.Image = Properties.Resources.OK;
				}
			}
		}

		private void PictureBoxLeftMouseClick(object sender, MouseEventArgs e)
		{
			if (e != null && e.Button == MouseButtons.Left)
			{
				ShowColorTooltip(sender as Control, e);
			}
		}

		private void PictureBoxRightMouseClick(object sender, MouseEventArgs e)
		{
			if (e != null && e.Button == MouseButtons.Left)
			{
				ShowColorTooltip(sender as Control, e);
			}
		}

		private void GroupBoxButtonsPaint(object sender, PaintEventArgs e)
		{
			RedrawBorder(e);
		}

		private void GroupBoxLeftButtonsPaint(object sender, PaintEventArgs e)
		{
			RedrawBorder(e);
		}

		private void ButtonPreviewMouseDown(object sender, MouseEventArgs e)
		{
			BayerImage bayerImage = new BayerImage(bayerPattern.BayerPattern, imageWidth, imageHeight);
			LoadImage(bayerImage.Image, pictureBoxRight, ref bitmapRight);
		}

		private void ButtonPreviewMouseUp(object sender, MouseEventArgs e)
		{
			LoadImage(image, pictureBoxLeft, ref bitmapLeft);
			LoadImage(filteredImage, pictureBoxRight, ref bitmapRight);
		}

		private void Zoom(int scaleFactor)
		{
			pictureBoxLeft.Image =
					BitmapHelpers.ResizeImage((Bitmap)image, new Size(image.Width * scaleFactor, image.Height * scaleFactor));
			pictureBoxRight.Image =
				BitmapHelpers.ResizeImage((Bitmap)filteredImage, new Size(filteredImage.Width * scaleFactor, filteredImage.Height * scaleFactor));

			pictureBoxLeft.SizeMode = PictureBoxSizeMode.AutoSize;
			pictureBoxRight.SizeMode = PictureBoxSizeMode.AutoSize;

			pictureBoxLeft.Anchor = AnchorStyles.Top | AnchorStyles.Left;
			pictureBoxRight.Anchor = AnchorStyles.Top | AnchorStyles.Left;
		}

		private void Unzoom()
		{
			pictureBoxLeft.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left;
			pictureBoxRight.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Left;

			pictureBoxLeft.SizeMode = PictureBoxSizeMode.CenterImage;
			pictureBoxRight.SizeMode = PictureBoxSizeMode.CenterImage;

			LoadImage(image, pictureBoxLeft, ref bitmapLeft);
			LoadImage(filteredImage, pictureBoxRight, ref bitmapRight);
		}

		private void ButtonZoomClick(object sender, EventArgs e)
		{
			if (isZoomed)
			{
				buttonZoom.Text = "Zoom";
				buttonPreview.Enabled = true;
				Unzoom();
			}
			else
			{
				buttonZoom.Text = "Unzoom";
				buttonPreview.Enabled = false;
				int scaleFactor = (int)comboBoxScaleFactor.SelectedItem;
				Zoom(scaleFactor);
			}

			isZoomed = !isZoomed;
		}

		private void PanelLeftScroll(object sender, ScrollEventArgs e)
		{
			if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
			{
				panelRight.HorizontalScroll.Value = e.NewValue;
			}
			else
			{
				panelRight.VerticalScroll.Value = e.NewValue;
			}
		}

		private void PanelRightScroll(object sender, ScrollEventArgs e)
		{
			if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
			{
				panelLeft.HorizontalScroll.Value = e.NewValue;
			}
			else
			{
				panelLeft.VerticalScroll.Value = e.NewValue;
			}
		}

		private void ComboBoxScaleFactorSelectedValueChanged(object sender, EventArgs e)
		{
			if (isZoomed)
			{
				int scaleFactor = (int)comboBoxScaleFactor.SelectedItem;

				Zoom(scaleFactor);
			}
		}

		private void ButtonRotateLeftClick(object sender, EventArgs e)
		{
			RotateImages(RotateFlipType.Rotate90FlipXY);
		}

		private void ButtonRotateRightClick(object sender, EventArgs e)
		{
			RotateImages(RotateFlipType.Rotate270FlipXY);
		}

		private void ButtonRotateUpClick(object sender, EventArgs e)
		{
			RotateImages(RotateFlipType.RotateNoneFlipXY);
		}

		private void ButtonRotateDownClick(object sender, EventArgs e)
		{
			RotateImages(RotateFlipType.RotateNoneFlipXY);
		}

		#endregion
	}
}
