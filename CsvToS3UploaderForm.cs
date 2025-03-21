using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using CsvHelper;
using System;
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
        private bool isAccessKeysVisible = false;
        private HttpClient _httpClient = new HttpClient();
        private string accessKeysFilePath;

        public CsvToS3Uploader()
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterScreen;
            Rectangle workingArea = Screen.GetWorkingArea(this);

            this.Width = Math.Min(workingArea.Width, 665);
            this.Height = Math.Min(workingArea.Height, 755);

            btnTransfer.Enabled = false;
            btnViewFileData.Enabled = false;
            btnTransferStop.Enabled = false;
            btnVisibleKey.Text = "👁️";
            btnVisibleKey.Font = new Font("Segoe UI", 15, FontStyle.Bold);
            btnVisibleKey.TextAlign = ContentAlignment.MiddleCenter;
            txtAccessKey.PasswordChar = '*';
            txtSecretKey.PasswordChar = '*';
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
            isAccessKeysVisible = !isAccessKeysVisible;

            txtAccessKey.PasswordChar = isAccessKeysVisible ? '\0' : '*';
            txtSecretKey.PasswordChar = isAccessKeysVisible ? '\0' : '*';

            btnVisibleKey.Text = isAccessKeysVisible ? "🔒" : "👁️";
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

            try
            {

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

                _cancellationTokenSource = new CancellationTokenSource();
                btnTransferStop.Enabled = true;

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
                            }
                        }
                    }
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
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "CSV files (*.csv)|*.csv";
                openFileDialog.Title = "Choose AWS Keys File";

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

                            var headers = csv.Context.Reader.HeaderRecord;

                            if (!headers.Contains("access_key") || !headers.Contains("secret_access_key") ||
                                !headers.Contains("bucket_name") || !headers.Contains("region"))
                            {
                                MessageBox.Show("The CSV format is wrong. Use the headings in the template in the same way!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            if (csv.Read())
                            {
                                txtAccessKey.Text = csv.GetField("access_key").Replace(" ", "").Trim();
                                txtSecretKey.Text = csv.GetField("secret_access_key").Replace(" ", "").Trim();
                                txtBucketName.Text = csv.GetField("bucket_name").Replace(" ", "").Trim();
                                txtRegion.Text = csv.GetField("region").Replace(" ", "").Trim();

                                MessageBox.Show("AWS Keys information uploaded successfully!", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnKeysCsvTemplateDownload_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
                saveFileDialog.Title = "Save Keys Template File";
                saveFileDialog.FileName = "keys_template.csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        byte[] fileContent = Encoding.UTF8.GetBytes(Properties.Resources.keys_template);

                        File.WriteAllBytes(saveFileDialog.FileName, fileContent);

                        MessageBox.Show("Keys Template file saved successfully!", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
