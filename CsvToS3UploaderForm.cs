using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TransferMediaCsvToS3App
{
    public partial class CsvToS3Uploader : Form
    {
        private CancellationTokenSource _cancellationTokenSource;
        private string selectedCsvFilePath;
        private bool isAwsKeysVisible = false;
        private bool isAzureKeysVisible = false;
        private HttpClient _httpClient = new HttpClient();

        public enum StorageProvider
        {
            AwsS3,
            AzureBlob
        }

        private StorageProvider currentProvider = StorageProvider.AwsS3;

        public CsvToS3Uploader()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
            Rectangle workingArea = Screen.GetWorkingArea(this);

            this.Width = Math.Min(workingArea.Width, 865);
            this.Height = Math.Min(workingArea.Height, 920);

            btnTransfer.Enabled = false;
            btnViewFileData.Enabled = false;
            btnTransferStop.Enabled = false;
            btnVisibleKey.Text = "👁️";
            btnVisibleKey.Font = new Font("Segoe UI", 15, FontStyle.Bold);
            btnVisibleKey.TextAlign = ContentAlignment.MiddleCenter;
            txtAccessKey.PasswordChar = '*';
            txtSecretKey.PasswordChar = '*';

            btnAzureVisibleKey.Text = "👁️";
            btnAzureVisibleKey.Font = new Font("Segoe UI", 15, FontStyle.Bold);
            btnAzureVisibleKey.TextAlign = ContentAlignment.MiddleCenter;
            txtAzureConnectionString.PasswordChar = '*';
        }
        private void btnSelectCsv_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "CSV files (*.csv)|*.csv",
                Title = "Select a CSV File"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFile = openFileDialog.FileName;

                if (Path.GetExtension(selectedFile).ToLower() != ".csv")
                {
                    MessageBox.Show("Only files with .CSV extension can be uploaded!", "Incorrect File Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                txtCsvPath.Text = selectedFile;
                selectedCsvFilePath = selectedFile;

                btnTransfer.Enabled = true;
                btnViewFileData.Enabled = true;

                MessageBox.Show("Uploaded file ready to transfer!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnVisibleKey_Click(object sender, EventArgs e)
        {
            isAwsKeysVisible = !isAwsKeysVisible;

            txtAccessKey.PasswordChar = isAwsKeysVisible ? '\0' : '*';
            txtSecretKey.PasswordChar = isAwsKeysVisible ? '\0' : '*';

            btnVisibleKey.Text = isAwsKeysVisible ? "🔒" : "👁️";
        }

        private void btnAzureVisibleKey_Click(object sender, EventArgs e)
        {
            isAzureKeysVisible = !isAzureKeysVisible;

            txtAzureConnectionString.PasswordChar = isAzureKeysVisible ? '\0' : '*';

            btnAzureVisibleKey.Text = isAzureKeysVisible ? "🔒" : "👁️";
        }

        private void btnViewFileData_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedCsvFilePath))
            {
                MessageBox.Show("Please select a CSV file!", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var viewModal = new ViewFileModalForm(selectedCsvFilePath);
            viewModal.ShowDialog();
        }

        private async void btnTransfer_Click(object sender, EventArgs e)
        {
            try
            {
                _cancellationTokenSource = new CancellationTokenSource();
                btnTransferStop.Enabled = true;

                if (currentProvider == StorageProvider.AwsS3)
                {
                    await TransferToAwsS3();
                }
                else
                {
                    await TransferToAzureBlob();
                }
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Transfer operation was stopped by the user!", "Process Stopped", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnTransferStop.Enabled = false;
            }
        }

        private async Task TransferToAwsS3()
        {
            string csvPath = txtCsvPath.Text;
            string accessKey = txtAccessKey.Text;
            string secretKey = txtSecretKey.Text;
            string region = txtRegion.Text;
            string bucketName = txtBucketName.Text;

            if (string.IsNullOrEmpty(csvPath) || string.IsNullOrEmpty(accessKey) || string.IsNullOrEmpty(secretKey) ||
                string.IsNullOrEmpty(region) || string.IsNullOrEmpty(bucketName))
            {
                MessageBox.Show("Please fill in all fields for AWS connection!", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IsValidAWSRegion(region))
            {
                MessageBox.Show($"Invalid AWS region: {region}. Please enter a valid region!", "Region Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var s3Client = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.GetBySystemName(region));

            if (!await ValidateS3Client(s3Client, bucketName))
            {
                return;
            }

            await ProcessCsvAndUpload(csvPath, async (record, fileStream, fileName) =>
            {
                bool exists = await CheckIfFileExists(s3Client, bucketName, fileName, record.RowNumber);

                if (!exists)
                {
                    await UploadToS3(s3Client, bucketName, fileName, fileStream, record.RowNumber);
                    Log(record.RowNumber, $"Uploaded: {fileName}");
                }
                else
                {
                    Log(record.RowNumber, $"Skipped (Exists): {fileName}");
                }
            });
        }

        private async Task TransferToAzureBlob()
        {
            string csvPath = txtCsvPath.Text;
            string connectionString = txtAzureConnectionString.Text;
            string containerName = txtAzureContainerName.Text;

            if (string.IsNullOrEmpty(csvPath) || string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(containerName))
            {
                MessageBox.Show("Please fill in all fields for Azure connection!", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var azureService = new AzureBlobStorageService(connectionString, containerName);

            if (!await ValidateAzureConnection(azureService))
            {
                return;
            }

            await ProcessCsvAndUpload(csvPath, async (record, fileStream, fileName) =>
            {
                bool exists = await azureService.CheckIfBlobExistsAsync(fileName);

                if (!exists)
                {
                    await azureService.UploadBlobAsync(fileName, fileStream, null, _cancellationTokenSource.Token);
                    Log(record.RowNumber, $"Uploaded: {fileName}");
                }
                else
                {
                    Log(record.RowNumber, $"Skipped (Exists): {fileName}");
                }
            });
        }

        private async Task ProcessCsvAndUpload(string csvPath, Func<CsvRecordModel, Stream, string, Task> uploadAction)
        {
            using (var stream = new FileStream(csvPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();

                var headers = csv.Context.Reader.HeaderRecord;

                if (!headers.Contains("media_url") || !headers.Contains("product_stock_code") ||
                    !headers.Contains("media_direction") || !headers.Contains("created_date"))
                {
                    MessageBox.Show("The CSV format is wrong. Use the headings in the template in the same way!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var records = csv.GetRecords<dynamic>();
                var processedRecords = CsvProcessor.ProcessCsvRecords(records);

                foreach (var record in processedRecords)
                {
                    if (_cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        Log(record.RowNumber, "The transfer has been canceled!");
                        break;
                    }

                    string mediaUrl = record.media_url;
                    string stockCode = CleanString(record.product_stock_code);
                    string mediaDirection = CleanString(record.media_direction);

                    if (!string.IsNullOrEmpty(mediaUrl))
                    {
                        var fileStream = await DownloadFile(record.RowNumber, mediaUrl);

                        if (fileStream != null)
                        {
                            string fileExtension = Path.GetExtension(mediaUrl);
                            string fileName;

                            if (record.IsLatestRecord)
                            {
                                fileName = $"{stockCode}_{mediaDirection}{fileExtension}";
                            }
                            else
                            {
                                fileName = $"_{stockCode}_{mediaDirection}_{Guid.NewGuid()}{fileExtension}";
                            }

                            await uploadAction(record, fileStream, fileName);
                        }
                    }
                }
            }
        }

        private async Task<bool> ValidateAzureConnection(AzureBlobStorageService azureService)
        {
            try
            {
                bool isValid = await azureService.ValidateConnectionAsync();
                if (!isValid)
                {
                    MessageBox.Show("Failed to connect to Azure Blob Storage with the entered information. Please check your information!", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return isValid;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error validating Azure connection: {ex.Message}", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private bool IsValidAWSRegion(string regionName)
        {
            return RegionEndpoint.EnumerableAllRegions.Any(region => region.SystemName.Equals(regionName, StringComparison.OrdinalIgnoreCase));
        }

        private string CleanString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return Regex.Replace(input, @"[^a-zA-Z0-9]", "");
        }

        private async Task<bool> ValidateS3Client(AmazonS3Client s3Client, string bucketName)
        {
            try
            {
                var response = await s3Client.ListBucketsAsync();
                if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                {
                    MessageBox.Show("Failed to connect to AWS S3 with the entered information. Please check your information!", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (!response.Buckets.Any(b => b.BucketName == bucketName))
                {
                    MessageBox.Show($"The bucket '{bucketName}' does not exist in AWS S3!", "Bucket Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to connect to AWS S3 with the entered information. Please check your information!", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private async Task<Stream> DownloadFile(int rowNumber, string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStreamAsync();
                }
                else
                {
                    Log(rowNumber, $"Failed to download: {url} - Status: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log(rowNumber, $"Error downloading {url}: {ex.Message}");
                return null;
            }
        }

        private async Task<bool> CheckIfFileExists(AmazonS3Client s3Client, string bucketName, string fileName, int rowNumber)
        {
            try
            {
                var request = new ListObjectsV2Request
                {
                    BucketName = bucketName,
                    Prefix = fileName
                };

                var response = await s3Client.ListObjectsV2Async(request);
                return response.S3Objects.Any(obj => obj.Key == fileName);
            }
            catch (Exception ex)
            {
                Log(rowNumber, $"Error checking file existence: {ex.Message}");
                return false;
            }
        }

        private async Task UploadToS3(AmazonS3Client s3Client, string bucketName, string fileName, Stream fileStream, int rowNumber)
        {
            try
            {
                var request = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = fileName,
                    InputStream = fileStream
                };

                await s3Client.PutObjectAsync(request);
            }
            catch (Exception ex)
            {
                Log(rowNumber, $"Error uploading {fileName}: {ex.Message}");
            }
        }

        private void btnTransferStop_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to stop the transfer process?", "Confirm Stop",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _cancellationTokenSource?.Cancel();
            }
        }

        private void btnDownloadCsvTemplate_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
                saveFileDialog.Title = "Save Template File";
                saveFileDialog.FileName = "template.csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        byte[] fileContent = Encoding.UTF8.GetBytes(Properties.Resources.template);

                        File.WriteAllBytes(saveFileDialog.FileName, fileContent);

                        MessageBox.Show("Template file saved successfully!", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnFillKeysFromCsv_Click(object sender, EventArgs e)
        {
            LoadKeysFromCsv((keys) =>
            {
                if (keys.ContainsKey("access_key") && keys.ContainsKey("secret_access_key") &&
                    keys.ContainsKey("bucket_name") && keys.ContainsKey("region"))
                {
                    txtAccessKey.Text = keys["access_key"];
                    txtSecretKey.Text = keys["secret_access_key"];
                    txtBucketName.Text = keys["bucket_name"];
                    txtRegion.Text = keys["region"];
                    return true;
                }
                return false;
            }, "AWS S3");
        }

        private void btnKeysCsvTemplateDownload_Click(object sender, EventArgs e)
        {
            DownloadKeysTemplate("keys_template.csv", Properties.Resources.keys_template, "AWS S3");
        }

        private void btnFillAzureKeysFromCsv_Click(object sender, EventArgs e)
        {
            LoadKeysFromCsv((keys) =>
            {
                if (keys.ContainsKey("connection_string") && keys.ContainsKey("container_name"))
                {
                    txtAzureConnectionString.Text = keys["connection_string"];
                    txtAzureContainerName.Text = keys["container_name"];
                    return true;
                }
                return false;
            }, "Azure Blob");
        }

        private void btnAzureKeysCsvTemplateDownload_Click(object sender, EventArgs e)
        {
            string azureTemplate = "connection_string,container_name\n";
            DownloadKeysTemplate("azure_keys_template.csv", azureTemplate, "Azure Blob");
        }

        private void LoadKeysFromCsv(Func<Dictionary<string, string>, bool> processKeys, string providerName)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "CSV files (*.csv)|*.csv";
                openFileDialog.Title = $"Choose {providerName} Keys File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string csvPath = openFileDialog.FileName;

                    if (Path.GetExtension(csvPath).ToLower() != ".csv")
                    {
                        MessageBox.Show("Only files with .CSV extension can be uploaded!", "Incorrect File Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        using (var reader = new StreamReader(csvPath))
                        using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                        {
                            csv.Read();
                            csv.ReadHeader();

                            if (csv.Read())
                            {
                                var headers = csv.Context.Reader.HeaderRecord;
                                var keys = new Dictionary<string, string>();

                                foreach (var header in headers)
                                {
                                    string value = csv.GetField(header)?.Replace(" ", "").Trim() ?? "";
                                    keys[header.ToLower()] = value;
                                }

                                if (processKeys(keys))
                                {
                                    MessageBox.Show($"{providerName} Keys information uploaded successfully!", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                else
                                {
                                    MessageBox.Show("The CSV format is wrong. Use the headings in the template in the same way!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("The CSV file is empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error processing file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void DownloadKeysTemplate(string fileName, string template, string providerName)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
                saveFileDialog.Title = $"Save {providerName} Keys Template File";
                saveFileDialog.FileName = fileName;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        byte[] fileContent = Encoding.UTF8.GetBytes(template);

                        File.WriteAllBytes(saveFileDialog.FileName, fileContent);

                        MessageBox.Show($"{providerName} Keys Template file saved successfully!", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void Log(int rowNumber, string message)
        {
            string logMessage = $"{DateTime.Now} ::::: [Row - {rowNumber}] ::::: {message}{Environment.NewLine}";

            rtbLogs.AppendText(logMessage);

            rtbLogs.SelectionStart = rtbLogs.Text.Length;
            rtbLogs.ScrollToCaret();

            SaveLogToFile(logMessage);
        }

        private void SaveLogToFile(string logMessage)
        {
            try
            {
                string exeDirectory = Path.GetDirectoryName(Application.ExecutablePath);

                string logDirectory = Path.Combine(exeDirectory, "Logs");

                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                string logFilePath = Path.Combine(logDirectory, $"log_{DateTime.Now:yyyy-MM-dd}.txt");
                File.AppendAllText(logFilePath, logMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error writing to log file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
