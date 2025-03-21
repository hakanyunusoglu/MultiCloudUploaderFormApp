namespace TransferMediaCsvToS3App
{
    partial class CsvToS3Uploader
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
            this.lblCsvPath = new System.Windows.Forms.Label();
            this.txtCsvPath = new System.Windows.Forms.TextBox();
            this.btnSelectCsv = new System.Windows.Forms.Button();
            this.btnTransfer = new System.Windows.Forms.Button();
            this.rtbLogs = new System.Windows.Forms.RichTextBox();
            this.btnViewFileData = new System.Windows.Forms.Button();
            this.btnVisibleKey = new System.Windows.Forms.Button();
            this.btnTransferStop = new System.Windows.Forms.Button();
            this.btnDownloadCsvTemplate = new System.Windows.Forms.Button();
            this.btnFillKeysFromCsv = new System.Windows.Forms.Button();
            this.btnKeysCsvTemplateDownload = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtAccessKey
            // 
            this.txtAccessKey.Location = new System.Drawing.Point(179, 55);
            this.txtAccessKey.Margin = new System.Windows.Forms.Padding(4);
            this.txtAccessKey.Name = "txtAccessKey";
            this.txtAccessKey.Size = new System.Drawing.Size(607, 22);
            this.txtAccessKey.TabIndex = 0;
            // 
            // lblAccessKey
            // 
            this.lblAccessKey.AutoSize = true;
            this.lblAccessKey.Location = new System.Drawing.Point(16, 58);
            this.lblAccessKey.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAccessKey.Name = "lblAccessKey";
            this.lblAccessKey.Size = new System.Drawing.Size(78, 16);
            this.lblAccessKey.TabIndex = 1;
            this.lblAccessKey.Text = "Access Key";
            // 
            // lblSecretKey
            // 
            this.lblSecretKey.AutoSize = true;
            this.lblSecretKey.Location = new System.Drawing.Point(16, 88);
            this.lblSecretKey.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSecretKey.Name = "lblSecretKey";
            this.lblSecretKey.Size = new System.Drawing.Size(120, 16);
            this.lblSecretKey.TabIndex = 3;
            this.lblSecretKey.Text = "Secret Access Key";
            // 
            // txtSecretKey
            // 
            this.txtSecretKey.Location = new System.Drawing.Point(179, 85);
            this.txtSecretKey.Margin = new System.Windows.Forms.Padding(4);
            this.txtSecretKey.Name = "txtSecretKey";
            this.txtSecretKey.Size = new System.Drawing.Size(607, 22);
            this.txtSecretKey.TabIndex = 2;
            // 
            // lblRegion
            // 
            this.lblRegion.AutoSize = true;
            this.lblRegion.Location = new System.Drawing.Point(16, 118);
            this.lblRegion.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRegion.Name = "lblRegion";
            this.lblRegion.Size = new System.Drawing.Size(51, 16);
            this.lblRegion.TabIndex = 5;
            this.lblRegion.Text = "Region";
            // 
            // txtRegion
            // 
            this.txtRegion.Location = new System.Drawing.Point(179, 115);
            this.txtRegion.Margin = new System.Windows.Forms.Padding(4);
            this.txtRegion.Name = "txtRegion";
            this.txtRegion.Size = new System.Drawing.Size(667, 22);
            this.txtRegion.TabIndex = 4;
            // 
            // lblBucketName
            // 
            this.lblBucketName.AutoSize = true;
            this.lblBucketName.Location = new System.Drawing.Point(16, 148);
            this.lblBucketName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBucketName.Name = "lblBucketName";
            this.lblBucketName.Size = new System.Drawing.Size(88, 16);
            this.lblBucketName.TabIndex = 7;
            this.lblBucketName.Text = "Bucket Name";
            // 
            // txtBucketName
            // 
            this.txtBucketName.Location = new System.Drawing.Point(179, 145);
            this.txtBucketName.Margin = new System.Windows.Forms.Padding(4);
            this.txtBucketName.Name = "txtBucketName";
            this.txtBucketName.Size = new System.Drawing.Size(667, 22);
            this.txtBucketName.TabIndex = 6;
            // 
            // lblCsvPath
            // 
            this.lblCsvPath.AutoSize = true;
            this.lblCsvPath.Location = new System.Drawing.Point(16, 178);
            this.lblCsvPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCsvPath.Name = "lblCsvPath";
            this.lblCsvPath.Size = new System.Drawing.Size(34, 16);
            this.lblCsvPath.TabIndex = 9;
            this.lblCsvPath.Text = "Path";
            // 
            // txtCsvPath
            // 
            this.txtCsvPath.Location = new System.Drawing.Point(179, 175);
            this.txtCsvPath.Margin = new System.Windows.Forms.Padding(4);
            this.txtCsvPath.Name = "txtCsvPath";
            this.txtCsvPath.ReadOnly = true;
            this.txtCsvPath.Size = new System.Drawing.Size(667, 22);
            this.txtCsvPath.TabIndex = 8;
            // 
            // btnSelectCsv
            // 
            this.btnSelectCsv.BackColor = System.Drawing.Color.PeachPuff;
            this.btnSelectCsv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectCsv.Font = new System.Drawing.Font("Arial Rounded MT Bold", 7.8F);
            this.btnSelectCsv.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSelectCsv.Location = new System.Drawing.Point(686, 216);
            this.btnSelectCsv.Name = "btnSelectCsv";
            this.btnSelectCsv.Size = new System.Drawing.Size(160, 37);
            this.btnSelectCsv.TabIndex = 10;
            this.btnSelectCsv.Text = "File Upload";
            this.btnSelectCsv.UseVisualStyleBackColor = false;
            this.btnSelectCsv.Click += new System.EventHandler(this.btnSelectCsv_Click);
            // 
            // btnTransfer
            // 
            this.btnTransfer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.btnTransfer.Enabled = false;
            this.btnTransfer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTransfer.Font = new System.Drawing.Font("Arial Rounded MT Bold", 7.8F);
            this.btnTransfer.ForeColor = System.Drawing.Color.Green;
            this.btnTransfer.Location = new System.Drawing.Point(686, 826);
            this.btnTransfer.Name = "btnTransfer";
            this.btnTransfer.Size = new System.Drawing.Size(160, 42);
            this.btnTransfer.TabIndex = 11;
            this.btnTransfer.Text = "TRANSFER";
            this.btnTransfer.UseVisualStyleBackColor = false;
            this.btnTransfer.Click += new System.EventHandler(this.btnTransfer_Click);
            // 
            // rtbLogs
            // 
            this.rtbLogs.Location = new System.Drawing.Point(19, 262);
            this.rtbLogs.Name = "rtbLogs";
            this.rtbLogs.Size = new System.Drawing.Size(827, 558);
            this.rtbLogs.TabIndex = 12;
            this.rtbLogs.Text = "";
            // 
            // btnViewFileData
            // 
            this.btnViewFileData.BackColor = System.Drawing.Color.LavenderBlush;
            this.btnViewFileData.Enabled = false;
            this.btnViewFileData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnViewFileData.Font = new System.Drawing.Font("Arial Rounded MT Bold", 7.8F);
            this.btnViewFileData.Location = new System.Drawing.Point(19, 216);
            this.btnViewFileData.Name = "btnViewFileData";
            this.btnViewFileData.Size = new System.Drawing.Size(170, 37);
            this.btnViewFileData.TabIndex = 13;
            this.btnViewFileData.Text = "View File Data";
            this.btnViewFileData.UseVisualStyleBackColor = false;
            this.btnViewFileData.Click += new System.EventHandler(this.btnViewFileData_Click);
            // 
            // btnVisibleKey
            // 
            this.btnVisibleKey.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnVisibleKey.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnVisibleKey.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnVisibleKey.Location = new System.Drawing.Point(793, 51);
            this.btnVisibleKey.Name = "btnVisibleKey";
            this.btnVisibleKey.Size = new System.Drawing.Size(53, 56);
            this.btnVisibleKey.TabIndex = 14;
            this.btnVisibleKey.UseVisualStyleBackColor = false;
            this.btnVisibleKey.Click += new System.EventHandler(this.btnVisibleKey_Click);
            // 
            // btnTransferStop
            // 
            this.btnTransferStop.BackColor = System.Drawing.Color.Red;
            this.btnTransferStop.Enabled = false;
            this.btnTransferStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTransferStop.Font = new System.Drawing.Font("Arial Rounded MT Bold", 7.8F);
            this.btnTransferStop.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnTransferStop.Location = new System.Drawing.Point(560, 826);
            this.btnTransferStop.Name = "btnTransferStop";
            this.btnTransferStop.Size = new System.Drawing.Size(120, 42);
            this.btnTransferStop.TabIndex = 15;
            this.btnTransferStop.Text = "STOP";
            this.btnTransferStop.UseVisualStyleBackColor = false;
            this.btnTransferStop.Click += new System.EventHandler(this.btnTransferStop_Click);
            // 
            // btnDownloadCsvTemplate
            // 
            this.btnDownloadCsvTemplate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnDownloadCsvTemplate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDownloadCsvTemplate.Font = new System.Drawing.Font("Arial Rounded MT Bold", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDownloadCsvTemplate.Location = new System.Drawing.Point(347, 216);
            this.btnDownloadCsvTemplate.Name = "btnDownloadCsvTemplate";
            this.btnDownloadCsvTemplate.Size = new System.Drawing.Size(175, 37);
            this.btnDownloadCsvTemplate.TabIndex = 16;
            this.btnDownloadCsvTemplate.Text = "Download Template";
            this.btnDownloadCsvTemplate.UseVisualStyleBackColor = false;
            this.btnDownloadCsvTemplate.Click += new System.EventHandler(this.btnDownloadCsvTemplate_Click);
            // 
            // btnFillKeysFromCsv
            // 
            this.btnFillKeysFromCsv.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.btnFillKeysFromCsv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFillKeysFromCsv.Font = new System.Drawing.Font("Arial Rounded MT Bold", 7.8F);
            this.btnFillKeysFromCsv.Location = new System.Drawing.Point(19, 11);
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
            this.btnKeysCsvTemplateDownload.Location = new System.Drawing.Point(210, 11);
            this.btnKeysCsvTemplateDownload.Name = "btnKeysCsvTemplateDownload";
            this.btnKeysCsvTemplateDownload.Size = new System.Drawing.Size(250, 37);
            this.btnKeysCsvTemplateDownload.TabIndex = 18;
            this.btnKeysCsvTemplateDownload.Text = "Download Keys Template";
            this.btnKeysCsvTemplateDownload.UseVisualStyleBackColor = false;
            this.btnKeysCsvTemplateDownload.Click += new System.EventHandler(this.btnKeysCsvTemplateDownload_Click);
            // 
            // CsvToS3Uploader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(858, 881);
            this.Controls.Add(this.btnKeysCsvTemplateDownload);
            this.Controls.Add(this.btnFillKeysFromCsv);
            this.Controls.Add(this.btnDownloadCsvTemplate);
            this.Controls.Add(this.btnTransferStop);
            this.Controls.Add(this.btnVisibleKey);
            this.Controls.Add(this.btnViewFileData);
            this.Controls.Add(this.rtbLogs);
            this.Controls.Add(this.btnTransfer);
            this.Controls.Add(this.btnSelectCsv);
            this.Controls.Add(this.lblCsvPath);
            this.Controls.Add(this.txtCsvPath);
            this.Controls.Add(this.lblBucketName);
            this.Controls.Add(this.txtBucketName);
            this.Controls.Add(this.lblRegion);
            this.Controls.Add(this.txtRegion);
            this.Controls.Add(this.lblSecretKey);
            this.Controls.Add(this.txtSecretKey);
            this.Controls.Add(this.lblAccessKey);
            this.Controls.Add(this.txtAccessKey);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CsvToS3Uploader";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CsvToS3Uploader";
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}

