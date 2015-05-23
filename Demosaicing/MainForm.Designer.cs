namespace Demosaicing
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.groupBoxButtons = new System.Windows.Forms.GroupBox();
			this.buttonDemosaic = new System.Windows.Forms.Button();
			this.buttonGrayscale = new System.Windows.Forms.Button();
			this.buttonBayer = new System.Windows.Forms.Button();
			this.pictureBoxStatus = new System.Windows.Forms.PictureBox();
			this.buttonSave = new System.Windows.Forms.Button();
			this.comboBoxDemosaicer = new System.Windows.Forms.ComboBox();
			this.labelDemosaicer = new System.Windows.Forms.Label();
			this.labelStatus = new System.Windows.Forms.Label();
			this.labelBayerPattern = new System.Windows.Forms.Label();
			this.comboBoxBayerPattern = new System.Windows.Forms.ComboBox();
			this.buttonBrowse = new System.Windows.Forms.Button();
			this.buttonShowMetrics = new System.Windows.Forms.Button();
			this.buttonAbout = new System.Windows.Forms.Button();
			this.toolTipColor = new System.Windows.Forms.ToolTip(this.components);
			this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
			this.panelRight = new System.Windows.Forms.Panel();
			this.pictureBoxRight = new System.Windows.Forms.PictureBox();
			this.panelLeft = new System.Windows.Forms.Panel();
			this.pictureBoxLeft = new System.Windows.Forms.PictureBox();
			this.groupBoxLeftButtons = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.buttonRotateDown = new System.Windows.Forms.Button();
			this.buttonRotateUp = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.comboBoxScaleFactor = new System.Windows.Forms.ComboBox();
			this.buttonRotateRight = new System.Windows.Forms.Button();
			this.buttonZoom = new System.Windows.Forms.Button();
			this.buttonRotateLeft = new System.Windows.Forms.Button();
			this.buttonPreview = new System.Windows.Forms.Button();
			this.groupBoxButtons.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxStatus)).BeginInit();
			this.tableLayoutPanel.SuspendLayout();
			this.panelRight.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxRight)).BeginInit();
			this.panelLeft.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxLeft)).BeginInit();
			this.groupBoxLeftButtons.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBoxButtons
			// 
			this.tableLayoutPanel.SetColumnSpan(this.groupBoxButtons, 3);
			this.groupBoxButtons.Controls.Add(this.buttonDemosaic);
			this.groupBoxButtons.Controls.Add(this.buttonGrayscale);
			this.groupBoxButtons.Controls.Add(this.buttonBayer);
			this.groupBoxButtons.Controls.Add(this.pictureBoxStatus);
			this.groupBoxButtons.Controls.Add(this.buttonSave);
			this.groupBoxButtons.Controls.Add(this.comboBoxDemosaicer);
			this.groupBoxButtons.Controls.Add(this.labelDemosaicer);
			this.groupBoxButtons.Controls.Add(this.labelStatus);
			this.groupBoxButtons.Controls.Add(this.labelBayerPattern);
			this.groupBoxButtons.Controls.Add(this.comboBoxBayerPattern);
			this.groupBoxButtons.Controls.Add(this.buttonBrowse);
			this.groupBoxButtons.Controls.Add(this.buttonShowMetrics);
			this.groupBoxButtons.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBoxButtons.Location = new System.Drawing.Point(3, 447);
			this.groupBoxButtons.Name = "groupBoxButtons";
			this.groupBoxButtons.Size = new System.Drawing.Size(879, 74);
			this.groupBoxButtons.TabIndex = 2;
			this.groupBoxButtons.TabStop = false;
			this.groupBoxButtons.Paint += new System.Windows.Forms.PaintEventHandler(this.GroupBoxButtonsPaint);
			// 
			// buttonDemosaic
			// 
			this.buttonDemosaic.Image = global::Demosaicing.Properties.Resources.Demosaic;
			this.buttonDemosaic.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.buttonDemosaic.Location = new System.Drawing.Point(297, 20);
			this.buttonDemosaic.Name = "buttonDemosaic";
			this.buttonDemosaic.Size = new System.Drawing.Size(96, 41);
			this.buttonDemosaic.TabIndex = 13;
			this.buttonDemosaic.Text = "Demosaic";
			this.buttonDemosaic.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.buttonDemosaic.UseVisualStyleBackColor = true;
			this.buttonDemosaic.Click += new System.EventHandler(this.ButtonDemosaicClick);
			// 
			// buttonGrayscale
			// 
			this.buttonGrayscale.Image = global::Demosaicing.Properties.Resources.Grayscale;
			this.buttonGrayscale.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.buttonGrayscale.Location = new System.Drawing.Point(115, 20);
			this.buttonGrayscale.Name = "buttonGrayscale";
			this.buttonGrayscale.Size = new System.Drawing.Size(100, 41);
			this.buttonGrayscale.TabIndex = 11;
			this.buttonGrayscale.Text = "Grayscale";
			this.buttonGrayscale.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.buttonGrayscale.UseVisualStyleBackColor = true;
			this.buttonGrayscale.Click += new System.EventHandler(this.ButtonGrayscaleClick);
			// 
			// buttonBayer
			// 
			this.buttonBayer.Image = global::Demosaicing.Properties.Resources.Bayer;
			this.buttonBayer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.buttonBayer.Location = new System.Drawing.Point(218, 20);
			this.buttonBayer.Name = "buttonBayer";
			this.buttonBayer.Size = new System.Drawing.Size(76, 41);
			this.buttonBayer.TabIndex = 12;
			this.buttonBayer.Text = "Bayer";
			this.buttonBayer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.buttonBayer.UseVisualStyleBackColor = true;
			this.buttonBayer.Click += new System.EventHandler(this.ButtonBayerClick);
			// 
			// pictureBoxStatus
			// 
			this.pictureBoxStatus.Image = global::Demosaicing.Properties.Resources.OK;
			this.pictureBoxStatus.InitialImage = null;
			this.pictureBoxStatus.Location = new System.Drawing.Point(810, 19);
			this.pictureBoxStatus.Name = "pictureBoxStatus";
			this.pictureBoxStatus.Size = new System.Drawing.Size(42, 42);
			this.pictureBoxStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictureBoxStatus.TabIndex = 31;
			this.pictureBoxStatus.TabStop = false;
			// 
			// buttonSave
			// 
			this.buttonSave.Image = global::Demosaicing.Properties.Resources.Save;
			this.buttonSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.buttonSave.Location = new System.Drawing.Point(528, 20);
			this.buttonSave.Name = "buttonSave";
			this.buttonSave.Size = new System.Drawing.Size(70, 41);
			this.buttonSave.TabIndex = 15;
			this.buttonSave.Text = "Save";
			this.buttonSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.buttonSave.UseVisualStyleBackColor = true;
			this.buttonSave.Click += new System.EventHandler(this.ButtonSaveClick);
			// 
			// comboBoxDemosaicer
			// 
			this.comboBoxDemosaicer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxDemosaicer.FormattingEnabled = true;
			this.comboBoxDemosaicer.Location = new System.Drawing.Point(677, 43);
			this.comboBoxDemosaicer.Name = "comboBoxDemosaicer";
			this.comboBoxDemosaicer.Size = new System.Drawing.Size(127, 21);
			this.comboBoxDemosaicer.TabIndex = 23;
			this.comboBoxDemosaicer.SelectedValueChanged += new System.EventHandler(this.ComboBoxDemosaicerSelectedValueChanged);
			// 
			// labelDemosaicer
			// 
			this.labelDemosaicer.AutoSize = true;
			this.labelDemosaicer.Location = new System.Drawing.Point(608, 46);
			this.labelDemosaicer.Name = "labelDemosaicer";
			this.labelDemosaicer.Size = new System.Drawing.Size(63, 13);
			this.labelDemosaicer.TabIndex = 22;
			this.labelDemosaicer.Text = "Demosaicer";
			// 
			// labelStatus
			// 
			this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.labelStatus.Location = new System.Drawing.Point(858, 20);
			this.labelStatus.Name = "labelStatus";
			this.labelStatus.Size = new System.Drawing.Size(293, 41);
			this.labelStatus.TabIndex = 31;
			this.labelStatus.Text = "Status Label";
			this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.labelStatus.TextChanged += new System.EventHandler(this.LabelStatusTextChanged);
			// 
			// labelBayerPattern
			// 
			this.labelBayerPattern.AutoSize = true;
			this.labelBayerPattern.Location = new System.Drawing.Point(630, 22);
			this.labelBayerPattern.Name = "labelBayerPattern";
			this.labelBayerPattern.Size = new System.Drawing.Size(41, 13);
			this.labelBayerPattern.TabIndex = 20;
			this.labelBayerPattern.Text = "Pattern";
			// 
			// comboBoxBayerPattern
			// 
			this.comboBoxBayerPattern.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxBayerPattern.FormattingEnabled = true;
			this.comboBoxBayerPattern.Location = new System.Drawing.Point(677, 19);
			this.comboBoxBayerPattern.Name = "comboBoxBayerPattern";
			this.comboBoxBayerPattern.Size = new System.Drawing.Size(127, 21);
			this.comboBoxBayerPattern.TabIndex = 21;
			this.comboBoxBayerPattern.SelectedValueChanged += new System.EventHandler(this.ComboBoxBayerPatternSelectedValueChanged);
			// 
			// buttonBrowse
			// 
			this.buttonBrowse.Image = global::Demosaicing.Properties.Resources.Browse;
			this.buttonBrowse.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.buttonBrowse.Location = new System.Drawing.Point(6, 20);
			this.buttonBrowse.Name = "buttonBrowse";
			this.buttonBrowse.Size = new System.Drawing.Size(94, 41);
			this.buttonBrowse.TabIndex = 10;
			this.buttonBrowse.Text = "Browse...";
			this.buttonBrowse.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.buttonBrowse.UseVisualStyleBackColor = true;
			this.buttonBrowse.Click += new System.EventHandler(this.ButtonBrowseClick);
			// 
			// buttonShowMetrics
			// 
			this.buttonShowMetrics.Image = global::Demosaicing.Properties.Resources.Bulb;
			this.buttonShowMetrics.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.buttonShowMetrics.Location = new System.Drawing.Point(402, 20);
			this.buttonShowMetrics.Name = "buttonShowMetrics";
			this.buttonShowMetrics.Size = new System.Drawing.Size(110, 41);
			this.buttonShowMetrics.TabIndex = 14;
			this.buttonShowMetrics.Text = "Show Metrics";
			this.buttonShowMetrics.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.buttonShowMetrics.UseVisualStyleBackColor = true;
			this.buttonShowMetrics.Click += new System.EventHandler(this.ButtonPsnrClick);
			// 
			// buttonAbout
			// 
			this.buttonAbout.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.buttonAbout.Image = global::Demosaicing.Properties.Resources.About;
			this.buttonAbout.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.buttonAbout.Location = new System.Drawing.Point(20, 373);
			this.buttonAbout.Name = "buttonAbout";
			this.buttonAbout.Size = new System.Drawing.Size(77, 41);
			this.buttonAbout.TabIndex = 32;
			this.buttonAbout.Text = "About";
			this.buttonAbout.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.buttonAbout.UseVisualStyleBackColor = true;
			this.buttonAbout.Click += new System.EventHandler(this.ButtonAboutClick);
			// 
			// tableLayoutPanel
			// 
			this.tableLayoutPanel.ColumnCount = 3;
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 130F));
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel.Controls.Add(this.panelRight, 2, 0);
			this.tableLayoutPanel.Controls.Add(this.panelLeft, 1, 0);
			this.tableLayoutPanel.Controls.Add(this.groupBoxLeftButtons, 0, 0);
			this.tableLayoutPanel.Controls.Add(this.groupBoxButtons, 0, 1);
			this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel.Location = new System.Drawing.Point(10, 10);
			this.tableLayoutPanel.Name = "tableLayoutPanel";
			this.tableLayoutPanel.RowCount = 2;
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
			this.tableLayoutPanel.Size = new System.Drawing.Size(885, 524);
			this.tableLayoutPanel.TabIndex = 33;
			// 
			// panelRight
			// 
			this.panelRight.AutoScroll = true;
			this.panelRight.AutoSize = true;
			this.panelRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelRight.Controls.Add(this.pictureBoxRight);
			this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelRight.Location = new System.Drawing.Point(510, 8);
			this.panelRight.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
			this.panelRight.Name = "panelRight";
			this.panelRight.Size = new System.Drawing.Size(372, 433);
			this.panelRight.TabIndex = 2;
			this.panelRight.Scroll += new System.Windows.Forms.ScrollEventHandler(this.PanelRightScroll);
			// 
			// pictureBoxRight
			// 
			this.pictureBoxRight.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBoxRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.pictureBoxRight.Location = new System.Drawing.Point(0, 0);
			this.pictureBoxRight.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
			this.pictureBoxRight.Name = "pictureBoxRight";
			this.pictureBoxRight.Padding = new System.Windows.Forms.Padding(10);
			this.pictureBoxRight.Size = new System.Drawing.Size(370, 431);
			this.pictureBoxRight.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictureBoxRight.TabIndex = 2;
			this.pictureBoxRight.TabStop = false;
			this.pictureBoxRight.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PictureBoxRightMouseClick);
			// 
			// panelLeft
			// 
			this.panelLeft.AutoScroll = true;
			this.panelLeft.AutoSize = true;
			this.panelLeft.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panelLeft.Controls.Add(this.pictureBoxLeft);
			this.panelLeft.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelLeft.Location = new System.Drawing.Point(133, 8);
			this.panelLeft.Margin = new System.Windows.Forms.Padding(3, 8, 3, 3);
			this.panelLeft.Name = "panelLeft";
			this.panelLeft.Size = new System.Drawing.Size(371, 433);
			this.panelLeft.TabIndex = 34;
			this.panelLeft.Scroll += new System.Windows.Forms.ScrollEventHandler(this.PanelLeftScroll);
			// 
			// pictureBoxLeft
			// 
			this.pictureBoxLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBoxLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.pictureBoxLeft.Location = new System.Drawing.Point(-1, 1);
			this.pictureBoxLeft.Margin = new System.Windows.Forms.Padding(3, 9, 3, 3);
			this.pictureBoxLeft.Name = "pictureBoxLeft";
			this.pictureBoxLeft.Padding = new System.Windows.Forms.Padding(10);
			this.pictureBoxLeft.Size = new System.Drawing.Size(369, 431);
			this.pictureBoxLeft.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.pictureBoxLeft.TabIndex = 1;
			this.pictureBoxLeft.TabStop = false;
			this.pictureBoxLeft.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PictureBoxLeftMouseClick);
			// 
			// groupBoxLeftButtons
			// 
			this.groupBoxLeftButtons.Controls.Add(this.tableLayoutPanel1);
			this.groupBoxLeftButtons.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBoxLeftButtons.Location = new System.Drawing.Point(3, 3);
			this.groupBoxLeftButtons.Name = "groupBoxLeftButtons";
			this.groupBoxLeftButtons.Size = new System.Drawing.Size(124, 438);
			this.groupBoxLeftButtons.TabIndex = 34;
			this.groupBoxLeftButtons.TabStop = false;
			this.groupBoxLeftButtons.Paint += new System.Windows.Forms.PaintEventHandler(this.GroupBoxLeftButtonsPaint);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.buttonAbout, 0, 3);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 300F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(118, 419);
			this.tableLayoutPanel1.TabIndex = 38;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.buttonRotateDown);
			this.panel1.Controls.Add(this.buttonRotateUp);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Controls.Add(this.comboBoxScaleFactor);
			this.panel1.Controls.Add(this.buttonRotateRight);
			this.panel1.Controls.Add(this.buttonZoom);
			this.panel1.Controls.Add(this.buttonRotateLeft);
			this.panel1.Controls.Add(this.buttonPreview);
			this.panel1.Location = new System.Drawing.Point(3, 37);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(106, 294);
			this.panel1.TabIndex = 0;
			// 
			// buttonRotateDown
			// 
			this.buttonRotateDown.Image = global::Demosaicing.Properties.Resources.Down;
			this.buttonRotateDown.Location = new System.Drawing.Point(59, 171);
			this.buttonRotateDown.Name = "buttonRotateDown";
			this.buttonRotateDown.Size = new System.Drawing.Size(41, 41);
			this.buttonRotateDown.TabIndex = 40;
			this.buttonRotateDown.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.buttonRotateDown.UseVisualStyleBackColor = true;
			this.buttonRotateDown.Click += new System.EventHandler(this.ButtonRotateDownClick);
			// 
			// buttonRotateUp
			// 
			this.buttonRotateUp.Image = global::Demosaicing.Properties.Resources.Up;
			this.buttonRotateUp.Location = new System.Drawing.Point(12, 171);
			this.buttonRotateUp.Name = "buttonRotateUp";
			this.buttonRotateUp.Size = new System.Drawing.Size(41, 41);
			this.buttonRotateUp.TabIndex = 39;
			this.buttonRotateUp.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.buttonRotateUp.UseVisualStyleBackColor = true;
			this.buttonRotateUp.Click += new System.EventHandler(this.ButtonRotateUpClick);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(9, 108);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(76, 13);
			this.label1.TabIndex = 38;
			this.label1.Text = "Rotate Images";
			// 
			// comboBoxScaleFactor
			// 
			this.comboBoxScaleFactor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboBoxScaleFactor.FormattingEnabled = true;
			this.comboBoxScaleFactor.Location = new System.Drawing.Point(12, 270);
			this.comboBoxScaleFactor.Name = "comboBoxScaleFactor";
			this.comboBoxScaleFactor.Size = new System.Drawing.Size(88, 21);
			this.comboBoxScaleFactor.TabIndex = 33;
			this.comboBoxScaleFactor.SelectedValueChanged += new System.EventHandler(this.ComboBoxScaleFactorSelectedValueChanged);
			// 
			// buttonRotateRight
			// 
			this.buttonRotateRight.Image = global::Demosaicing.Properties.Resources.Right;
			this.buttonRotateRight.Location = new System.Drawing.Point(59, 124);
			this.buttonRotateRight.Name = "buttonRotateRight";
			this.buttonRotateRight.Size = new System.Drawing.Size(41, 41);
			this.buttonRotateRight.TabIndex = 37;
			this.buttonRotateRight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.buttonRotateRight.UseVisualStyleBackColor = true;
			this.buttonRotateRight.Click += new System.EventHandler(this.ButtonRotateRightClick);
			// 
			// buttonZoom
			// 
			this.buttonZoom.Image = global::Demosaicing.Properties.Resources.Zoom;
			this.buttonZoom.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.buttonZoom.Location = new System.Drawing.Point(12, 223);
			this.buttonZoom.Name = "buttonZoom";
			this.buttonZoom.Size = new System.Drawing.Size(88, 41);
			this.buttonZoom.TabIndex = 34;
			this.buttonZoom.Text = "Zoom";
			this.buttonZoom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.buttonZoom.UseVisualStyleBackColor = true;
			this.buttonZoom.Click += new System.EventHandler(this.ButtonZoomClick);
			// 
			// buttonRotateLeft
			// 
			this.buttonRotateLeft.Image = global::Demosaicing.Properties.Resources.Left;
			this.buttonRotateLeft.Location = new System.Drawing.Point(12, 124);
			this.buttonRotateLeft.Name = "buttonRotateLeft";
			this.buttonRotateLeft.Size = new System.Drawing.Size(41, 41);
			this.buttonRotateLeft.TabIndex = 33;
			this.buttonRotateLeft.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.buttonRotateLeft.UseVisualStyleBackColor = true;
			this.buttonRotateLeft.Click += new System.EventHandler(this.ButtonRotateLeftClick);
			// 
			// buttonPreview
			// 
			this.buttonPreview.Image = global::Demosaicing.Properties.Resources.Pattern;
			this.buttonPreview.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.buttonPreview.Location = new System.Drawing.Point(12, 55);
			this.buttonPreview.Name = "buttonPreview";
			this.buttonPreview.Size = new System.Drawing.Size(88, 41);
			this.buttonPreview.TabIndex = 33;
			this.buttonPreview.Text = "Preview Pattern";
			this.buttonPreview.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.buttonPreview.UseVisualStyleBackColor = true;
			this.buttonPreview.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ButtonPreviewMouseDown);
			this.buttonPreview.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ButtonPreviewMouseUp);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(905, 544);
			this.Controls.Add(this.tableLayoutPanel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MainForm";
			this.Padding = new System.Windows.Forms.Padding(10);
			this.Text = "Demosaicing";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.Resize += new System.EventHandler(this.MainForm_Resize);
			this.groupBoxButtons.ResumeLayout(false);
			this.groupBoxButtons.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxStatus)).EndInit();
			this.tableLayoutPanel.ResumeLayout(false);
			this.tableLayoutPanel.PerformLayout();
			this.panelRight.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxRight)).EndInit();
			this.panelLeft.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxLeft)).EndInit();
			this.groupBoxLeftButtons.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button buttonBrowse;
		private System.Windows.Forms.Button buttonShowMetrics;
		private System.Windows.Forms.GroupBox groupBoxButtons;
		private System.Windows.Forms.Label labelBayerPattern;
		private System.Windows.Forms.ComboBox comboBoxBayerPattern;
		private System.Windows.Forms.Label labelStatus;
		private System.Windows.Forms.ComboBox comboBoxDemosaicer;
		private System.Windows.Forms.Label labelDemosaicer;
		private System.Windows.Forms.Button buttonSave;
		private System.Windows.Forms.PictureBox pictureBoxStatus;
		private System.Windows.Forms.Button buttonDemosaic;
		private System.Windows.Forms.Button buttonBayer;
		private System.Windows.Forms.Button buttonGrayscale;
		private System.Windows.Forms.Button buttonAbout;
		private System.Windows.Forms.ToolTip toolTipColor;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
		private System.Windows.Forms.GroupBox groupBoxLeftButtons;
		private System.Windows.Forms.Button buttonPreview;
		private System.Windows.Forms.Panel panelLeft;
		private System.Windows.Forms.PictureBox pictureBoxLeft;
		private System.Windows.Forms.Panel panelRight;
		private System.Windows.Forms.PictureBox pictureBoxRight;
		private System.Windows.Forms.ComboBox comboBoxScaleFactor;
		private System.Windows.Forms.Button buttonZoom;
		private System.Windows.Forms.Button buttonRotateRight;
		private System.Windows.Forms.Button buttonRotateLeft;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button buttonRotateDown;
		private System.Windows.Forms.Button buttonRotateUp;
	}
}

