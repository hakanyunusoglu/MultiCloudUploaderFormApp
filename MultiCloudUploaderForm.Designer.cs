namespace MediaCloudUploaderFormApp
{
    partial class MultiCloudUploaderForm
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
            this.txtAccessKey = new System.Windows.Forms.TextBox();
            this.lblAccessKey = new System.Windows.Forms.Label();
            this.lblSecretKey = new System.Windows.Forms.Label();
            this.txtSecretKey = new System.Windows.Forms.TextBox();
            this.lblRegion = new System.Windows.Forms.Label();
            this.txtRegion = new System.Windows.Forms.TextBox();
            this.lblBucketName = new System.Windows.Forms.Label();
            this.txtBucketName = new System.Windows.Forms.TextBox();
            this.btnVisibleKey = new System.Windows.Forms.Button();
            this.btnFillKeysFromCsv = new System.Windows.Forms.Button();
            this.btnKeysCsvTemplateDownload = new System.Windows.Forms.Button();
            this.txtAzureBlobUrl = new System.Windows.Forms.TextBox();
            this.lblAzureBlobUrl = new System.Windows.Forms.Label();
            this.txtAzureSasToken = new System.Windows.Forms.TextBox();
            this.lblAzureSasToken = new System.Windows.Forms.Label();
            this.lblAzureContainerName = new System.Windows.Forms.Label();
            this.txtAzureContainerName = new System.Windows.Forms.TextBox();
            this.btnAzureVisibleKey = new System.Windows.Forms.Button();
            this.btnFillAzureKeysFromCsv = new System.Windows.Forms.Button();
            this.btnAzureKeysCsvTemplateDownload = new System.Windows.Forms.Button();
            this.lblCsvPath = new System.Windows.Forms.Label();
            this.txtCsvPath = new System.Windows.Forms.TextBox();
            this.btnSelectCsv = new System.Windows.Forms.Button();
            this.btnViewFileData = new System.Windows.Forms.Button();
            this.btnDownloadCsvTemplate = new System.Windows.Forms.Button();
            this.btnTransfer = new System.Windows.Forms.Button();
            this.rtbLogs = new System.Windows.Forms.RichTextBox();
            this.btnTransferStop = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageProviderSelection = new System.Windows.Forms.TabPage();
            this.btnSelectProvider = new System.Windows.Forms.Button();
            this.lblProviderSelection = new System.Windows.Forms.Label();
            this.rbAzureBlob = new System.Windows.Forms.RadioButton();
            this.rbAwsS3 = new System.Windows.Forms.RadioButton();
            this.tabPageAWS = new System.Windows.Forms.TabPage();
            this.gbAwsMediaFilter = new System.Windows.Forms.GroupBox();
            this.rbAwsLatestMedia = new System.Windows.Forms.RadioButton();
            this.rbAwsHistoricalMedia = new System.Windows.Forms.RadioButton();
            this.rbAwsAllMedia = new System.Windows.Forms.RadioButton();
            this.tabPageAzure = new System.Windows.Forms.TabPage();
            this.lblAzureFolderPath = new System.Windows.Forms.Label();
            this.txtAzureFolderPath = new System.Windows.Forms.TextBox();
            this.gbAzureMediaFilter = new System.Windows.Forms.GroupBox();
            this.rbAzureLatestMedia = new System.Windows.Forms.RadioButton();
            this.rbAzureHistoricalMedia = new System.Windows.Forms.RadioButton();
            this.rbAzureAllMedia = new System.Windows.Forms.RadioButton();
            this.tabPageFile = new System.Windows.Forms.TabPage();
            this.tabPageTransfer = new System.Windows.Forms.TabPage();
            this.btnClearLog = new System.Windows.Forms.Button();

            this.gbDataTransferMode = new System.Windows.Forms.GroupBox();
            this.rbCreateWithNewList = new System.Windows.Forms.RadioButton();
            this.rbCopyFromExisting = new System.Windows.Forms.RadioButton();
            this.gbProviderSelection = new System.Windows.Forms.GroupBox();


            this.tabControl.SuspendLayout();
            this.tabPageProviderSelection.SuspendLayout();
            this.tabPageAWS.SuspendLayout();
            this.gbAwsMediaFilter.SuspendLayout();
            this.tabPageAzure.SuspendLayout();
            this.gbAzureMediaFilter.SuspendLayout();
            this.tabPageFile.SuspendLayout();
            this.tabPageTransfer.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtAccessKey
            // 
            this.txtAccessKey.Location = new System.Drawing.Point(160, 45);
            this.txtAccessKey.Margin = new System.Windows.Forms.Padding(4);
            this.txtAccessKey.Name = "txtAccessKey";
            this.txtAccessKey.PasswordChar = '*';
            this.txtAccessKey.Size = new System.Drawing.Size(590, 22);
            this.txtAccessKey.TabIndex = 0;
            // 
            // lblAccessKey
            // 
            this.lblAccessKey.AutoSize = true;
            this.lblAccessKey.Location = new System.Drawing.Point(20, 48);
            this.lblAccessKey.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAccessKey.Name = "lblAccessKey";
            this.lblAccessKey.Size = new System.Drawing.Size(78, 16);
            this.lblAccessKey.TabIndex = 1;
            this.lblAccessKey.Text = "Access Key";
            // 
            // lblSecretKey
            // 
            this.lblSecretKey.AutoSize = true;
            this.lblSecretKey.Location = new System.Drawing.Point(20, 78);
            this.lblSecretKey.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSecretKey.Name = "lblSecretKey";
            this.lblSecretKey.Size = new System.Drawing.Size(120, 16);
            this.lblSecretKey.TabIndex = 3;
            this.lblSecretKey.Text = "Secret Access Key";
            // 
            // txtSecretKey
            // 
            this.txtSecretKey.Location = new System.Drawing.Point(160, 75);
            this.txtSecretKey.Margin = new System.Windows.Forms.Padding(4);
            this.txtSecretKey.Name = "txtSecretKey";
            this.txtSecretKey.PasswordChar = '*';
            this.txtSecretKey.Size = new System.Drawing.Size(590, 22);
            this.txtSecretKey.TabIndex = 2;
            // 
            // lblRegion
            // 
            this.lblRegion.AutoSize = true;
            this.lblRegion.Location = new System.Drawing.Point(20, 108);
            this.lblRegion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRegion.Name = "lblRegion";
            this.lblRegion.Size = new System.Drawing.Size(51, 16);
            this.lblRegion.TabIndex = 5;
            this.lblRegion.Text = "Region";
            // 
            // txtRegion
            // 
            this.txtRegion.Location = new System.Drawing.Point(160, 105);
            this.txtRegion.Margin = new System.Windows.Forms.Padding(4);
            this.txtRegion.Name = "txtRegion";
            this.txtRegion.Size = new System.Drawing.Size(650, 22);
            this.txtRegion.TabIndex = 4;
            // 
            // lblBucketName
            // 
            this.lblBucketName.AutoSize = true;
            this.lblBucketName.Location = new System.Drawing.Point(20, 138);
            this.lblBucketName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBucketName.Name = "lblBucketName";
            this.lblBucketName.Size = new System.Drawing.Size(88, 16);
            this.lblBucketName.TabIndex = 7;
            this.lblBucketName.Text = "Bucket Name";
            // 
            // txtBucketName
            // 
            this.txtBucketName.Location = new System.Drawing.Point(160, 135);
            this.txtBucketName.Margin = new System.Windows.Forms.Padding(4);
            this.txtBucketName.Name = "txtBucketName";
            this.txtBucketName.Size = new System.Drawing.Size(650, 22);
            this.txtBucketName.TabIndex = 6;
            // 
            // btnVisibleKey
            // 
            this.btnVisibleKey.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnVisibleKey.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnVisibleKey.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnVisibleKey.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.btnVisibleKey.Location = new System.Drawing.Point(757, 45);
            this.btnVisibleKey.Name = "btnVisibleKey";
            this.btnVisibleKey.Size = new System.Drawing.Size(53, 52);
            this.btnVisibleKey.TabIndex = 14;
            this.btnVisibleKey.Text = "👁️";
            this.btnVisibleKey.UseVisualStyleBackColor = false;
            this.btnVisibleKey.Click += new System.EventHandler(this.btnVisibleKey_Click);
            // 
            // btnFillKeysFromCsv
            // 
            this.btnFillKeysFromCsv.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnFillKeysFromCsv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFillKeysFromCsv.Font = new System.Drawing.Font("Arial Rounded MT Bold", 7.8F);
            this.btnFillKeysFromCsv.Location = new System.Drawing.Point(20, 244);
            this.btnFillKeysFromCsv.Name = "btnFillKeysFromCsv";
            this.btnFillKeysFromCsv.Size = new System.Drawing.Size(170, 37);
            this.btnFillKeysFromCsv.TabIndex = 17;
            this.btnFillKeysFromCsv.Text = "Fill Keys From Csv";
            this.btnFillKeysFromCsv.UseVisualStyleBackColor = false;
            this.btnFillKeysFromCsv.Click += new System.EventHandler(this.btnFillKeysFromCsv_Click);
            // 
            // btnKeysCsvTemplateDownload
            // 
            this.btnKeysCsvTemplateDownload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnKeysCsvTemplateDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnKeysCsvTemplateDownload.Font = new System.Drawing.Font("Arial Rounded MT Bold", 7.8F);
            this.btnKeysCsvTemplateDownload.Location = new System.Drawing.Point(210, 244);
            this.btnKeysCsvTemplateDownload.Name = "btnKeysCsvTemplateDownload";
            this.btnKeysCsvTemplateDownload.Size = new System.Drawing.Size(250, 37);
            this.btnKeysCsvTemplateDownload.TabIndex = 18;
            this.btnKeysCsvTemplateDownload.Text = "Download Keys Template";
            this.btnKeysCsvTemplateDownload.UseVisualStyleBackColor = false;
            this.btnKeysCsvTemplateDownload.Click += new System.EventHandler(this.btnKeysCsvTemplateDownload_Click);
            // 
            // txtAzureBlobUrl
            // 
            this.txtAzureBlobUrl.Location = new System.Drawing.Point(160, 45);
            this.txtAzureBlobUrl.Margin = new System.Windows.Forms.Padding(4);
            this.txtAzureBlobUrl.Name = "txtAzureBlobUrl";
            this.txtAzureBlobUrl.PasswordChar = '*';
            this.txtAzureBlobUrl.Size = new System.Drawing.Size(590, 22);
            this.txtAzureBlobUrl.TabIndex = 0;
            // 
            // lblAzureBlobUrl
            // 
            this.lblAzureBlobUrl.AutoSize = true;
            this.lblAzureBlobUrl.Location = new System.Drawing.Point(20, 48);
            this.lblAzureBlobUrl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAzureBlobUrl.Name = "lblAzureBlobUrl";
            this.lblAzureBlobUrl.Size = new System.Drawing.Size(65, 16);
            this.lblAzureBlobUrl.TabIndex = 1;
            this.lblAzureBlobUrl.Text = "Blob URL";
            // 
            // txtAzureSasToken
            // 
            this.txtAzureSasToken.Location = new System.Drawing.Point(160, 75);
            this.txtAzureSasToken.Margin = new System.Windows.Forms.Padding(4);
            this.txtAzureSasToken.Name = "txtAzureSasToken";
            this.txtAzureSasToken.PasswordChar = '*';
            this.txtAzureSasToken.Size = new System.Drawing.Size(590, 22);
            this.txtAzureSasToken.TabIndex = 2;
            // 
            // lblAzureSasToken
            // 
            this.lblAzureSasToken.AutoSize = true;
            this.lblAzureSasToken.Location = new System.Drawing.Point(20, 78);
            this.lblAzureSasToken.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAzureSasToken.Name = "lblAzureSasToken";
            this.lblAzureSasToken.Size = new System.Drawing.Size(76, 16);
            this.lblAzureSasToken.TabIndex = 3;
            this.lblAzureSasToken.Text = "SAS Token";
            // 
            // lblAzureContainerName
            // 
            this.lblAzureContainerName.AutoSize = true;
            this.lblAzureContainerName.Location = new System.Drawing.Point(20, 108);
            this.lblAzureContainerName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAzureContainerName.Name = "lblAzureContainerName";
            this.lblAzureContainerName.Size = new System.Drawing.Size(104, 16);
            this.lblAzureContainerName.TabIndex = 5;
            this.lblAzureContainerName.Text = "Container Name";
            // 
            // txtAzureContainerName
            // 
            this.txtAzureContainerName.Location = new System.Drawing.Point(160, 105);
            this.txtAzureContainerName.Margin = new System.Windows.Forms.Padding(4);
            this.txtAzureContainerName.Name = "txtAzureContainerName";
            this.txtAzureContainerName.Size = new System.Drawing.Size(650, 22);
            this.txtAzureContainerName.TabIndex = 4;
            // 
            // btnAzureVisibleKey
            // 
            this.btnAzureVisibleKey.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnAzureVisibleKey.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnAzureVisibleKey.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAzureVisibleKey.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.btnAzureVisibleKey.Location = new System.Drawing.Point(757, 45);
            this.btnAzureVisibleKey.Name = "btnAzureVisibleKey";
            this.btnAzureVisibleKey.Size = new System.Drawing.Size(53, 52);
            this.btnAzureVisibleKey.TabIndex = 14;
            this.btnAzureVisibleKey.Text = "👁️";
            this.btnAzureVisibleKey.UseVisualStyleBackColor = false;
            this.btnAzureVisibleKey.Click += new System.EventHandler(this.btnAzureVisibleKey_Click);
            // 
            // btnFillAzureKeysFromCsv
            // 
            this.btnFillAzureKeysFromCsv.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnFillAzureKeysFromCsv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFillAzureKeysFromCsv.Font = new System.Drawing.Font("Arial Rounded MT Bold", 7.8F);
            this.btnFillAzureKeysFromCsv.Location = new System.Drawing.Point(20, 244);
            this.btnFillAzureKeysFromCsv.Name = "btnFillAzureKeysFromCsv";
            this.btnFillAzureKeysFromCsv.Size = new System.Drawing.Size(170, 37);
            this.btnFillAzureKeysFromCsv.TabIndex = 17;
            this.btnFillAzureKeysFromCsv.Text = "Fill Keys From Csv";
            this.btnFillAzureKeysFromCsv.UseVisualStyleBackColor = false;
            this.btnFillAzureKeysFromCsv.Click += new System.EventHandler(this.btnFillAzureKeysFromCsv_Click);
            // 
            // btnAzureKeysCsvTemplateDownload
            // 
            this.btnAzureKeysCsvTemplateDownload.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnAzureKeysCsvTemplateDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAzureKeysCsvTemplateDownload.Font = new System.Drawing.Font("Arial Rounded MT Bold", 7.8F);
            this.btnAzureKeysCsvTemplateDownload.Location = new System.Drawing.Point(210, 244);
            this.btnAzureKeysCsvTemplateDownload.Name = "btnAzureKeysCsvTemplateDownload";
            this.btnAzureKeysCsvTemplateDownload.Size = new System.Drawing.Size(250, 37);
            this.btnAzureKeysCsvTemplateDownload.TabIndex = 18;
            this.btnAzureKeysCsvTemplateDownload.Text = "Download Keys Template";
            this.btnAzureKeysCsvTemplateDownload.UseVisualStyleBackColor = false;
            this.btnAzureKeysCsvTemplateDownload.Click += new System.EventHandler(this.btnAzureKeysCsvTemplateDownload_Click);
            // 
            // lblCsvPath
            // 
            this.lblCsvPath.AutoSize = true;
            this.lblCsvPath.Location = new System.Drawing.Point(20, 50);
            this.lblCsvPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCsvPath.Name = "lblCsvPath";
            this.lblCsvPath.Size = new System.Drawing.Size(34, 16);
            this.lblCsvPath.TabIndex = 9;
            this.lblCsvPath.Text = "Path";
            // 
            // txtCsvPath
            // 
            this.txtCsvPath.Location = new System.Drawing.Point(100, 47);
            this.txtCsvPath.Margin = new System.Windows.Forms.Padding(4);
            this.txtCsvPath.Name = "txtCsvPath";
            this.txtCsvPath.ReadOnly = true;
            this.txtCsvPath.Size = new System.Drawing.Size(700, 22);
            this.txtCsvPath.TabIndex = 8;
            // 
            // btnSelectCsv
            // 
            this.btnSelectCsv.BackColor = System.Drawing.Color.PeachPuff;
            this.btnSelectCsv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectCsv.Font = new System.Drawing.Font("Arial Rounded MT Bold", 7.8F);
            this.btnSelectCsv.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSelectCsv.Location = new System.Drawing.Point(640, 100);
            this.btnSelectCsv.Name = "btnSelectCsv";
            this.btnSelectCsv.Size = new System.Drawing.Size(160, 37);
            this.btnSelectCsv.TabIndex = 10;
            this.btnSelectCsv.Text = "File Upload";
            this.btnSelectCsv.UseVisualStyleBackColor = false;
            this.btnSelectCsv.Click += new System.EventHandler(this.btnSelectCsv_Click);
            // 
            // btnViewFileData
            // 
            this.btnViewFileData.BackColor = System.Drawing.Color.LavenderBlush;
            this.btnViewFileData.Enabled = false;
            this.btnViewFileData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewFileData.Font = new System.Drawing.Font("Arial Rounded MT Bold", 7.8F);
            this.btnViewFileData.Location = new System.Drawing.Point(100, 100);
            this.btnViewFileData.Name = "btnViewFileData";
            this.btnViewFileData.Size = new System.Drawing.Size(170, 37);
            this.btnViewFileData.TabIndex = 13;
            this.btnViewFileData.Text = "View File Data";
            this.btnViewFileData.UseVisualStyleBackColor = false;
            this.btnViewFileData.Click += new System.EventHandler(this.btnViewFileData_Click);
            // 
            // btnDownloadCsvTemplate
            // 
            this.btnDownloadCsvTemplate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnDownloadCsvTemplate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownloadCsvTemplate.Font = new System.Drawing.Font("Arial Rounded MT Bold", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDownloadCsvTemplate.Location = new System.Drawing.Point(359, 100);
            this.btnDownloadCsvTemplate.Name = "btnDownloadCsvTemplate";
            this.btnDownloadCsvTemplate.Size = new System.Drawing.Size(175, 37);
            this.btnDownloadCsvTemplate.TabIndex = 16;
            this.btnDownloadCsvTemplate.Text = "Download Template";
            this.btnDownloadCsvTemplate.UseVisualStyleBackColor = false;
            this.btnDownloadCsvTemplate.Click += new System.EventHandler(this.btnDownloadCsvTemplate_Click);
            // 
            // btnTransfer
            // 
            this.btnTransfer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnTransfer.Enabled = false;
            this.btnTransfer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTransfer.Font = new System.Drawing.Font("Arial Rounded MT Bold", 7.8F);
            this.btnTransfer.ForeColor = System.Drawing.Color.Green;
            this.btnTransfer.Location = new System.Drawing.Point(650, 669);
            this.btnTransfer.Name = "btnTransfer";
            this.btnTransfer.Size = new System.Drawing.Size(159, 42);
            this.btnTransfer.TabIndex = 11;
            this.btnTransfer.Text = "TRANSFER";
            this.btnTransfer.UseVisualStyleBackColor = false;
            this.btnTransfer.Click += new System.EventHandler(this.btnTransfer_Click);
            // 
            // rtbLogs
            // 
            this.rtbLogs.Location = new System.Drawing.Point(10, 10);
            this.rtbLogs.Name = "rtbLogs";
            this.rtbLogs.Size = new System.Drawing.Size(800, 650);
            this.rtbLogs.TabIndex = 12;
            this.rtbLogs.Text = "";
            // 
            // btnTransferStop
            // 
            this.btnTransferStop.BackColor = System.Drawing.Color.Red;
            this.btnTransferStop.Enabled = false;
            this.btnTransferStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTransferStop.Font = new System.Drawing.Font("Arial Rounded MT Bold", 7.8F);
            this.btnTransferStop.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnTransferStop.Location = new System.Drawing.Point(519, 669);
            this.btnTransferStop.Name = "btnTransferStop";
            this.btnTransferStop.Size = new System.Drawing.Size(116, 42);
            this.btnTransferStop.TabIndex = 15;
            this.btnTransferStop.Text = "STOP";
            this.btnTransferStop.UseVisualStyleBackColor = false;
            this.btnTransferStop.Click += new System.EventHandler(this.btnTransferStop_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageProviderSelection);
            this.tabControl.Controls.Add(this.tabPageAWS);
            this.tabControl.Controls.Add(this.tabPageAzure);
            this.tabControl.Controls.Add(this.tabPageFile);
            this.tabControl.Controls.Add(this.tabPageTransfer);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(825, 751);
            this.tabControl.TabIndex = 19;
            // 
            // tabPageProviderSelection
            // 
            this.tabPageProviderSelection.Controls.Add(this.btnSelectProvider);
            this.tabPageProviderSelection.Controls.Add(this.lblProviderSelection);
            this.tabPageProviderSelection.Controls.Add(this.rbAzureBlob);
            this.tabPageProviderSelection.Controls.Add(this.rbAwsS3);
            this.tabPageProviderSelection.Controls.Add(this.gbDataTransferMode);
            this.tabPageProviderSelection.Controls.Add(this.gbProviderSelection);
            this.tabPageProviderSelection.Location = new System.Drawing.Point(4, 25);
            this.tabPageProviderSelection.Name = "tabPageProviderSelection";
            this.tabPageProviderSelection.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProviderSelection.Size = new System.Drawing.Size(817, 722);
            this.tabPageProviderSelection.TabIndex = 0;
            this.tabPageProviderSelection.Text = "Provider Selection";
            this.tabPageProviderSelection.UseVisualStyleBackColor = true;
            // 
            // btnSelectProvider
            // 
            this.btnSelectProvider.BackColor = System.Drawing.Color.LightBlue;
            this.btnSelectProvider.Font = new System.Drawing.Font("Arial Rounded MT Bold", 10F);
            this.btnSelectProvider.Location = new System.Drawing.Point(300, 490);
            this.btnSelectProvider.Name = "btnSelectProvider";
            this.btnSelectProvider.Size = new System.Drawing.Size(200, 50);
            this.btnSelectProvider.TabIndex = 3;
            this.btnSelectProvider.Text = "Continue";
            this.btnSelectProvider.UseVisualStyleBackColor = false;
            this.btnSelectProvider.Click += new System.EventHandler(this.btnSelectProvider_Click);
            // 
            // lblProviderSelection
            // 
            this.lblProviderSelection.AutoSize = true;
            this.lblProviderSelection.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.lblProviderSelection.Location = new System.Drawing.Point(296, 200);
            this.lblProviderSelection.Name = "lblProviderSelection";
            this.lblProviderSelection.Size = new System.Drawing.Size(276, 31);
            this.lblProviderSelection.TabIndex = 2;
            this.lblProviderSelection.Text = "Select Cloud Provider";
            // 
            // rbAzureBlob
            // 
            this.rbAzureBlob.AutoSize = true;
            this.rbAzureBlob.Location = new System.Drawing.Point(20, 60);
            this.rbAzureBlob.Name = "rbAzureBlob";
            this.rbAzureBlob.Size = new System.Drawing.Size(144, 20);
            this.rbAzureBlob.TabIndex = 1;
            this.rbAzureBlob.TabStop = true;
            this.rbAzureBlob.Text = "Azure Blob Storage";
            this.rbAzureBlob.UseVisualStyleBackColor = true;
            this.rbAzureBlob.CheckedChanged += new System.EventHandler(this.rbAzureBlob_CheckedChanged);
            // 
            // rbAwsS3
            // 
            this.rbAwsS3.AutoSize = true;
            this.rbAwsS3.Location = new System.Drawing.Point(20, 30);
            this.rbAwsS3.Name = "rbAwsS3";
            this.rbAwsS3.Size = new System.Drawing.Size(78, 20);
            this.rbAwsS3.TabIndex = 0;
            this.rbAwsS3.TabStop = true;
            this.rbAwsS3.Text = "AWS S3";
            this.rbAwsS3.UseVisualStyleBackColor = true;
            this.rbAwsS3.CheckedChanged += new System.EventHandler(this.rbAwsS3_CheckedChanged);
            // 
            // tabPageAWS
            // 
            this.tabPageAWS.Controls.Add(this.btnKeysCsvTemplateDownload);
            this.tabPageAWS.Controls.Add(this.btnFillKeysFromCsv);
            this.tabPageAWS.Controls.Add(this.btnVisibleKey);
            this.tabPageAWS.Controls.Add(this.lblAccessKey);
            this.tabPageAWS.Controls.Add(this.txtAccessKey);
            this.tabPageAWS.Controls.Add(this.lblSecretKey);
            this.tabPageAWS.Controls.Add(this.txtSecretKey);
            this.tabPageAWS.Controls.Add(this.lblRegion);
            this.tabPageAWS.Controls.Add(this.txtRegion);
            this.tabPageAWS.Controls.Add(this.lblBucketName);
            this.tabPageAWS.Controls.Add(this.txtBucketName);
            this.tabPageAWS.Controls.Add(this.gbAwsMediaFilter);
            this.tabPageAWS.Location = new System.Drawing.Point(4, 25);
            this.tabPageAWS.Name = "tabPageAWS";
            this.tabPageAWS.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAWS.Size = new System.Drawing.Size(817, 722);
            this.tabPageAWS.TabIndex = 1;
            this.tabPageAWS.Text = "AWS S3";
            this.tabPageAWS.UseVisualStyleBackColor = true;
            // 
            // gbAwsMediaFilter
            // 
            this.gbAwsMediaFilter.Controls.Add(this.rbAwsLatestMedia);
            this.gbAwsMediaFilter.Controls.Add(this.rbAwsHistoricalMedia);
            this.gbAwsMediaFilter.Controls.Add(this.rbAwsAllMedia);
            this.gbAwsMediaFilter.Location = new System.Drawing.Point(161, 169);
            this.gbAwsMediaFilter.Name = "gbAwsMediaFilter";
            this.gbAwsMediaFilter.Size = new System.Drawing.Size(649, 60);
            this.gbAwsMediaFilter.TabIndex = 21;
            this.gbAwsMediaFilter.TabStop = false;
            this.gbAwsMediaFilter.Text = "Media Filter";

            // gbDataTransferMode
            this.gbDataTransferMode.Controls.Add(this.rbCopyFromExisting);
            this.gbDataTransferMode.Controls.Add(this.rbCreateWithNewList);
            this.gbDataTransferMode.Location = new System.Drawing.Point(225, 370);
            this.gbDataTransferMode.Name = "gbDataTransferMode";
            this.gbDataTransferMode.Size = new System.Drawing.Size(350, 100);
            this.gbDataTransferMode.TabIndex = 6;
            this.gbDataTransferMode.TabStop = false;
            this.gbDataTransferMode.Text = "Data Transfer Mode";

            // gbProviderSelection
            this.gbProviderSelection.Controls.Add(this.rbAzureBlob);
            this.gbProviderSelection.Controls.Add(this.rbAwsS3);
            this.gbProviderSelection.Location = new System.Drawing.Point(225, 250);
            this.gbProviderSelection.Name = "gbProviderSelection";
            this.gbProviderSelection.Size = new System.Drawing.Size(350, 100);
            this.gbProviderSelection.TabIndex = 5;
            this.gbProviderSelection.TabStop = false;
            this.gbProviderSelection.Text = "Cloud Provider Selection";

            // rbCreateWithNewList
            this.rbCreateWithNewList.AutoSize = true;
            this.rbCreateWithNewList.Checked = true;
            this.rbCreateWithNewList.Location = new System.Drawing.Point(20, 30);
            this.rbCreateWithNewList.Name = "rbCreateWithNewList";
            this.rbCreateWithNewList.Size = new System.Drawing.Size(153, 20);
            this.rbCreateWithNewList.TabIndex = 0;
            this.rbCreateWithNewList.TabStop = true;
            this.rbCreateWithNewList.Text = "Create With New List";
            this.rbCreateWithNewList.UseVisualStyleBackColor = true;

            // rbCopyFromExisting
            this.rbCopyFromExisting.AutoSize = true;
            this.rbCopyFromExisting.Location = new System.Drawing.Point(20, 60);
            this.rbCopyFromExisting.Name = "rbCopyFromExisting";
            this.rbCopyFromExisting.Size = new System.Drawing.Size(147, 20);
            this.rbCopyFromExisting.TabIndex = 1;
            this.rbCopyFromExisting.Text = "Copy From Existing";
            this.rbCopyFromExisting.UseVisualStyleBackColor = true;


            // 
            // rbAwsLatestMedia
            // 
            this.rbAwsLatestMedia.AutoSize = true;
            this.rbAwsLatestMedia.Location = new System.Drawing.Point(147, 25);
            this.rbAwsLatestMedia.Name = "rbAwsLatestMedia";
            this.rbAwsLatestMedia.Size = new System.Drawing.Size(183, 20);
            this.rbAwsLatestMedia.TabIndex = 1;
            this.rbAwsLatestMedia.Text = "Only Upload Latest Media";
            this.rbAwsLatestMedia.UseVisualStyleBackColor = true;
            // 
            // rbAwsHistoricalMedia
            // 
            this.rbAwsHistoricalMedia.AutoSize = true;
            this.rbAwsHistoricalMedia.Location = new System.Drawing.Point(377, 25);
            this.rbAwsHistoricalMedia.Name = "rbAwsHistoricalMedia";
            this.rbAwsHistoricalMedia.Size = new System.Drawing.Size(203, 20);
            this.rbAwsHistoricalMedia.TabIndex = 2;
            this.rbAwsHistoricalMedia.Text = "Only Upload Historical Media";
            this.rbAwsHistoricalMedia.UseVisualStyleBackColor = true;
            // 
            // rbAwsAllMedia
            // 
            this.rbAwsAllMedia.AutoSize = true;
            this.rbAwsAllMedia.Checked = true;
            this.rbAwsAllMedia.Location = new System.Drawing.Point(15, 25);
            this.rbAwsAllMedia.Name = "rbAwsAllMedia";
            this.rbAwsAllMedia.Size = new System.Drawing.Size(84, 20);
            this.rbAwsAllMedia.TabIndex = 0;
            this.rbAwsAllMedia.TabStop = true;
            this.rbAwsAllMedia.Text = "All Media";
            this.rbAwsAllMedia.UseVisualStyleBackColor = true;
            // 
            // tabPageAzure
            // 
            this.tabPageAzure.Controls.Add(this.btnAzureKeysCsvTemplateDownload);
            this.tabPageAzure.Controls.Add(this.btnFillAzureKeysFromCsv);
            this.tabPageAzure.Controls.Add(this.btnAzureVisibleKey);
            this.tabPageAzure.Controls.Add(this.lblAzureBlobUrl);
            this.tabPageAzure.Controls.Add(this.txtAzureBlobUrl);
            this.tabPageAzure.Controls.Add(this.lblAzureSasToken);
            this.tabPageAzure.Controls.Add(this.txtAzureSasToken);
            this.tabPageAzure.Controls.Add(this.lblAzureContainerName);
            this.tabPageAzure.Controls.Add(this.txtAzureContainerName);
            this.tabPageAzure.Controls.Add(this.lblAzureFolderPath);
            this.tabPageAzure.Controls.Add(this.txtAzureFolderPath);
            this.tabPageAzure.Controls.Add(this.gbAzureMediaFilter);
            this.tabPageAzure.Location = new System.Drawing.Point(4, 25);
            this.tabPageAzure.Name = "tabPageAzure";
            this.tabPageAzure.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageAzure.Size = new System.Drawing.Size(817, 722);
            this.tabPageAzure.TabIndex = 2;
            this.tabPageAzure.Text = "Azure Blob";
            this.tabPageAzure.UseVisualStyleBackColor = true;
            // 
            // lblAzureFolderPath
            // 
            this.lblAzureFolderPath.AutoSize = true;
            this.lblAzureFolderPath.Location = new System.Drawing.Point(20, 138);
            this.lblAzureFolderPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAzureFolderPath.Name = "lblAzureFolderPath";
            this.lblAzureFolderPath.Size = new System.Drawing.Size(76, 16);
            this.lblAzureFolderPath.TabIndex = 7;
            this.lblAzureFolderPath.Text = "Folder Path";
            // 
            // txtAzureFolderPath
            // 
            this.txtAzureFolderPath.Location = new System.Drawing.Point(160, 135);
            this.txtAzureFolderPath.Margin = new System.Windows.Forms.Padding(4);
            this.txtAzureFolderPath.Name = "txtAzureFolderPath";
            this.txtAzureFolderPath.Size = new System.Drawing.Size(650, 22);
            this.txtAzureFolderPath.TabIndex = 8;
            // 
            // gbAzureMediaFilter
            // 
            this.gbAzureMediaFilter.Controls.Add(this.rbAzureLatestMedia);
            this.gbAzureMediaFilter.Controls.Add(this.rbAzureHistoricalMedia);
            this.gbAzureMediaFilter.Controls.Add(this.rbAzureAllMedia);
            this.gbAzureMediaFilter.Location = new System.Drawing.Point(161, 169);
            this.gbAzureMediaFilter.Name = "gbAzureMediaFilter";
            this.gbAzureMediaFilter.Size = new System.Drawing.Size(649, 60);
            this.gbAzureMediaFilter.TabIndex = 21;
            this.gbAzureMediaFilter.TabStop = false;
            this.gbAzureMediaFilter.Text = "Media Filter";
            // 
            // rbAzureLatestMedia
            // 
            this.rbAzureLatestMedia.AutoSize = true;
            this.rbAzureLatestMedia.Location = new System.Drawing.Point(147, 25);
            this.rbAzureLatestMedia.Name = "rbAzureLatestMedia";
            this.rbAzureLatestMedia.Size = new System.Drawing.Size(183, 20);
            this.rbAzureLatestMedia.TabIndex = 1;
            this.rbAzureLatestMedia.Text = "Only Upload Latest Media";
            this.rbAzureLatestMedia.UseVisualStyleBackColor = true;
            // 
            // rbAzureHistoricalMedia
            // 
            this.rbAzureHistoricalMedia.AutoSize = true;
            this.rbAzureHistoricalMedia.Location = new System.Drawing.Point(377, 25);
            this.rbAzureHistoricalMedia.Name = "rbAzureHistoricalMedia";
            this.rbAzureHistoricalMedia.Size = new System.Drawing.Size(203, 20);
            this.rbAzureHistoricalMedia.TabIndex = 2;
            this.rbAzureHistoricalMedia.Text = "Only Upload Historical Media";
            this.rbAzureHistoricalMedia.UseVisualStyleBackColor = true;
            // 
            // rbAzureAllMedia
            // 
            this.rbAzureAllMedia.AutoSize = true;
            this.rbAzureAllMedia.Checked = true;
            this.rbAzureAllMedia.Location = new System.Drawing.Point(15, 25);
            this.rbAzureAllMedia.Name = "rbAzureAllMedia";
            this.rbAzureAllMedia.Size = new System.Drawing.Size(84, 20);
            this.rbAzureAllMedia.TabIndex = 0;
            this.rbAzureAllMedia.TabStop = true;
            this.rbAzureAllMedia.Text = "All Media";
            this.rbAzureAllMedia.UseVisualStyleBackColor = true;
            // 
            // tabPageFile
            // 
            this.tabPageFile.Controls.Add(this.btnDownloadCsvTemplate);
            this.tabPageFile.Controls.Add(this.btnSelectCsv);
            this.tabPageFile.Controls.Add(this.btnViewFileData);
            this.tabPageFile.Controls.Add(this.lblCsvPath);
            this.tabPageFile.Controls.Add(this.txtCsvPath);
            this.tabPageFile.Location = new System.Drawing.Point(4, 25);
            this.tabPageFile.Name = "tabPageFile";
            this.tabPageFile.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFile.Size = new System.Drawing.Size(817, 722);
            this.tabPageFile.TabIndex = 3;
            this.tabPageFile.Text = "Dosya İşlemleri";
            this.tabPageFile.UseVisualStyleBackColor = true;
            // 
            // tabPageTransfer
            // 
            this.tabPageTransfer.Controls.Add(this.rtbLogs);
            this.tabPageTransfer.Controls.Add(this.btnTransfer);
            this.tabPageTransfer.Controls.Add(this.btnClearLog);
            this.tabPageTransfer.Controls.Add(this.btnTransferStop);
            this.tabPageTransfer.Location = new System.Drawing.Point(4, 25);
            this.tabPageTransfer.Name = "tabPageTransfer";
            this.tabPageTransfer.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTransfer.Size = new System.Drawing.Size(817, 722);
            this.tabPageTransfer.TabIndex = 4;
            this.tabPageTransfer.Text = "Transfer";
            this.tabPageTransfer.UseVisualStyleBackColor = true;
            // 
            // btnClearLog
            // 
            this.btnClearLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnClearLog.Enabled = false;
            this.btnClearLog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearLog.Font = new System.Drawing.Font("Arial Rounded MT Bold", 7.8F);
            this.btnClearLog.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnClearLog.Location = new System.Drawing.Point(10, 669);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(116, 42);
            this.btnClearLog.TabIndex = 15;
            this.btnClearLog.Text = "CLEAR";
            this.btnClearLog.UseVisualStyleBackColor = false;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
            // 
            // MultiCloudUploaderForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(843, 773);
            this.Controls.Add(this.tabControl);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MultiCloudUploaderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Multi Cloud Media Uploader";
            this.tabControl.ResumeLayout(false);
            this.tabPageProviderSelection.ResumeLayout(false);
            this.tabPageProviderSelection.PerformLayout();
            this.tabPageAWS.ResumeLayout(false);
            this.tabPageAWS.PerformLayout();
            this.gbAwsMediaFilter.ResumeLayout(false);
            this.gbAwsMediaFilter.PerformLayout();
            this.tabPageAzure.ResumeLayout(false);
            this.tabPageAzure.PerformLayout();
            this.gbAzureMediaFilter.ResumeLayout(false);
            this.gbAzureMediaFilter.PerformLayout();
            this.tabPageFile.ResumeLayout(false);
            this.tabPageFile.PerformLayout();
            this.tabPageTransfer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox txtAccessKey;
        private System.Windows.Forms.Label lblAccessKey;
        private System.Windows.Forms.Label lblSecretKey;
        private System.Windows.Forms.TextBox txtSecretKey;
        private System.Windows.Forms.Label lblRegion;
        private System.Windows.Forms.TextBox txtRegion;
        private System.Windows.Forms.Label lblBucketName;
        private System.Windows.Forms.TextBox txtBucketName;
        private System.Windows.Forms.Label lblCsvPath;
        private System.Windows.Forms.TextBox txtCsvPath;
        private System.Windows.Forms.Button btnSelectCsv;
        private System.Windows.Forms.Button btnTransfer;
        private System.Windows.Forms.RichTextBox rtbLogs;
        private System.Windows.Forms.Button btnViewFileData;
        private System.Windows.Forms.Button btnVisibleKey;
        private System.Windows.Forms.Button btnTransferStop;
        private System.Windows.Forms.Button btnDownloadCsvTemplate;
        private System.Windows.Forms.Button btnFillKeysFromCsv;
        private System.Windows.Forms.Button btnKeysCsvTemplateDownload;
        private System.Windows.Forms.GroupBox gbAwsMediaFilter;
        private System.Windows.Forms.RadioButton rbAwsLatestMedia;
        private System.Windows.Forms.RadioButton rbAwsHistoricalMedia;
        private System.Windows.Forms.RadioButton rbAwsAllMedia;


        // Azure için yeni kontroller eklendi
        private System.Windows.Forms.TextBox txtAzureBlobUrl;
        private System.Windows.Forms.Label lblAzureBlobUrl;
        private System.Windows.Forms.TextBox txtAzureSasToken;
        private System.Windows.Forms.Label lblAzureSasToken;

        private System.Windows.Forms.Label lblAzureContainerName;
        private System.Windows.Forms.TextBox txtAzureContainerName;
        private System.Windows.Forms.Button btnAzureVisibleKey;
        private System.Windows.Forms.Button btnFillAzureKeysFromCsv;
        private System.Windows.Forms.Button btnAzureKeysCsvTemplateDownload;
        private System.Windows.Forms.Label lblAzureFolderPath;
        private System.Windows.Forms.TextBox txtAzureFolderPath;
        private System.Windows.Forms.GroupBox gbAzureMediaFilter;
        private System.Windows.Forms.RadioButton rbAzureLatestMedia;
        private System.Windows.Forms.RadioButton rbAzureHistoricalMedia;
        private System.Windows.Forms.RadioButton rbAzureAllMedia;


        // Tab kontrolü
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageAWS;
        private System.Windows.Forms.TabPage tabPageAzure;
        private System.Windows.Forms.TabPage tabPageFile;
        private System.Windows.Forms.TabPage tabPageTransfer;
        private System.Windows.Forms.TabPage tabPageProviderSelection;

        // Provider Selection Components
        private System.Windows.Forms.RadioButton rbAwsS3;
        private System.Windows.Forms.RadioButton rbAzureBlob;
        private System.Windows.Forms.Label lblProviderSelection;
        private System.Windows.Forms.Button btnSelectProvider;
        private System.Windows.Forms.Button btnClearLog;

        private System.Windows.Forms.GroupBox gbDataTransferMode;
        private System.Windows.Forms.RadioButton rbCreateWithNewList;
        private System.Windows.Forms.RadioButton rbCopyFromExisting;
        private System.Windows.Forms.GroupBox gbProviderSelection;
    }
}

