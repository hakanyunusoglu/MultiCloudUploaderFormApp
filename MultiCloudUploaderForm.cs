using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaCloudUploaderFormApp
{
    public partial class MultiCloudUploaderForm : Form
    {
        private CancellationTokenSource _cancellationTokenSource;
        private string selectedCsvFilePath;
        private bool isAwsKeysVisible = false;
        private bool isAzureKeysVisible = false;
        private HttpClient _httpClient = new HttpClient();
        private M3u8DownloadService _m3u8Service;

        public enum StorageProvider
        {
            None,
            AwsS3,
            AzureBlob
        }

        private StorageProvider currentProvider = StorageProvider.None;
        private DataTransferMode currentTransferMode = DataTransferMode.CreateWithNewList;
        public MultiCloudUploaderForm()
        {
            try
            {
                InitializeComponent();

                tabControl.TabPages.Clear();
                tabControl.TabPages.Add(tabPageProviderSelection);
                tabControl.SelectedTab = tabPageProviderSelection;

                rbAwsAllMedia.Checked = true;
                rbAzureAllMedia.Checked = true;
                rbCreateWithNewList.Checked = true;

                rbCreateWithNewList.CheckedChanged += (s, e) =>
                {
                    if (rbCreateWithNewList.Checked)
                        currentTransferMode = DataTransferMode.CreateWithNewList;
                };
                rbCopyFromExisting.CheckedChanged += (s, e) =>
                {
                    if (rbCopyFromExisting.Checked)
                        currentTransferMode = DataTransferMode.CopyFromExisting;
                };

                _m3u8Service = new M3u8DownloadService(_httpClient);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Form başlatılamadı: {ex.ToString()}");
                Environment.Exit(1);
            }
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

            txtAzureBlobUrl.PasswordChar = isAzureKeysVisible ? '\0' : '*';
            txtAzureSasToken.PasswordChar = isAzureKeysVisible ? '\0' : '*';

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
                Log(0, $"Transfer initiated. Current Provider: {currentProvider}, Mode: {currentTransferMode}");

                if (currentProvider == StorageProvider.None)
                {
                    MessageBox.Show("Please select a cloud provider first!", "Provider Not Selected",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _cancellationTokenSource = new CancellationTokenSource();
                btnTransferStop.Enabled = true;
                btnClearLog.Enabled = false;
                tabControl.TabPages.Remove(tabPageProviderSelection);

                if (currentTransferMode == DataTransferMode.CreateWithNewList)
                {
                    if (currentProvider == StorageProvider.AwsS3)
                    {
                        await TransferToAwsS3();
                    }
                    else
                    {
                        await TransferToAzureBlob();
                    }
                }
                else
                {
                    await TransferExistingMedia();
                }
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Transfer operation was stopped by the user!", "Process Stopped", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Log(0, "Transfer operation was stopped by the user!");
                tabControl.TabPages.Insert(0, tabPageProviderSelection);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnTransferStop.Enabled = false;
                btnClearLog.Enabled = true;
                tabControl.TabPages.Insert(0, tabPageProviderSelection);
            }
        }

        private async Task TransferExistingMedia()
        {
            string csvPath = txtCsvPath.Text;

            if (string.IsNullOrEmpty(csvPath))
            {
                MessageBox.Show("Please select a CSV file!", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int uploadedCount = 0;
            int skippedCount = 0;
            int failedCount = 0;
            DateTime startTime = DateTime.Now;

            using (var stream = new FileStream(csvPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();

                var headers = csv.Context.Reader.HeaderRecord;

                if (!headers.Contains("media_name") || !headers.Contains("media_extension") ||
                    !headers.Contains("media_url") || !headers.Contains("is_converted_m3u8") ||
                    !headers.Contains("m3u8_media_url"))
                {
                    MessageBox.Show("The CSV format is wrong for existing media transfer!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var records = csv.GetRecords<dynamic>();
                var processedRecords = CsvProcessor.ProcessCsvRecordsToExistingMedias(records);

                Log(0, $"Found {processedRecords.Count} existing media records to process.");

                foreach (var record in processedRecords)
                {
                    if (_cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        Log(record.RowNumber, "The transfer has been canceled!");
                        break;
                    }

                    try
                    {
                        if (record.is_converted_m3u8 && !string.IsNullOrEmpty(record.m3u8_media_url))
                        {
                            // M3U8 transfer işlemi
                            await TransferM3u8Media(record);
                            bool success = await TransferSingleMedia(record);
                            if (success)
                                uploadedCount++;
                            else
                                skippedCount++;
                        }
                        else
                        {
                            // Normal media transfer işlemi
                            bool success = await TransferSingleMedia(record);
                            if (success)
                                uploadedCount++;
                            else
                                skippedCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log(record.RowNumber, $"Failed to transfer: {record.media_name} - Error: {ex.Message}");
                        failedCount++;
                    }
                }
            }

            TimeSpan duration = DateTime.Now - startTime;
            string formattedDuration = duration.ToString(@"hh\:mm\:ss");

            Log(0, $"--------------------------------------------------");
            Log(0, $"EXISTING MEDIA TRANSFER COMPLETED");
            Log(0, $"Duration: {formattedDuration}");
            Log(0, $"Total Files Processed: {uploadedCount + skippedCount + failedCount}");
            Log(0, $"Successfully Uploaded: {uploadedCount}");
            Log(0, $"Skipped (Already Exists): {skippedCount}");
            if (failedCount > 0)
                Log(0, $"Failed: {failedCount}");
            Log(0, $"--------------------------------------------------");

            MessageBox.Show(
                $"Transfer completed!\n\nUploaded: {uploadedCount}\nSkipped: {skippedCount}\nFailed: {failedCount}",
                "Transfer Results",
                MessageBoxButtons.OK,
                failedCount > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
        }

        private string BuildFileName(ExistingMediaModel record)
        {
            string mediaName = record.media_name?.Trim();
            string extension = record.media_extension?.Trim();

            if (string.IsNullOrEmpty(mediaName))
            {
                throw new ArgumentException("Media name cannot be empty");
            }

            // media_name'de zaten extension var mı kontrol et
            if (Path.HasExtension(mediaName))
            {
                // media_name'de extension var, bunu kullan
                return mediaName;
            }
            else
            {
                // media_name'de extension yok, media_extension'dan ekle
                if (string.IsNullOrEmpty(extension))
                {
                    throw new ArgumentException($"Extension is required for media: {mediaName}");
                }

                // Extension'da nokta var mı kontrol et, yoksa ekle
                if (!extension.StartsWith("."))
                {
                    extension = "." + extension;
                }

                return mediaName + extension;
            }
        }

        private async Task<bool> TransferSingleMedia(ExistingMediaModel record)
        {
            string fileName;

            try
            {
                fileName = BuildFileName(record);
            }
            catch (ArgumentException ex)
            {
                Log(record.RowNumber, $"Invalid file name configuration: {ex.Message}");
                return false;
            }

            Stream mediaStream = null;

            if (record.media_url.StartsWith("data:") || (!record.media_url.StartsWith("http://") && !record.media_url.StartsWith("https://")))
            {
                // Base64 data
                try
                {
                    string base64Data = record.media_url;
                    if (base64Data.Contains(","))
                    {
                        base64Data = base64Data.Split(',')[1];
                    }

                    byte[] bytes = Convert.FromBase64String(base64Data);
                    mediaStream = new MemoryStream(bytes);

                    Log(record.RowNumber, $"Base64 data converted for: {fileName}");
                }
                catch (Exception ex)
                {
                    Log(record.RowNumber, $"Failed to convert base64 data for: {fileName} - {ex.Message}");
                    return false;
                }
            }
            else
            {
                // HTTP/HTTPS URL
                try
                {
                    var response = await _httpClient.GetAsync(record.media_url);
                    if (response.IsSuccessStatusCode)
                    {
                        mediaStream = await response.Content.ReadAsStreamAsync();
                        Log(record.RowNumber, $"Downloaded from URL: {fileName}");
                    }
                    else
                    {
                        Log(record.RowNumber, $"Failed to download from URL: {record.media_url} - Status: {response.StatusCode}");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Log(record.RowNumber, $"Error downloading from URL: {record.media_url} - {ex.Message}");
                    return false;
                }
            }

            if (mediaStream != null)
            {
                try
                {
                    if (currentProvider == StorageProvider.AwsS3)
                    {
                        var awsService = CreateAwsService();
                        bool exists = await awsService.CheckIfFileExistsAsync(fileName);

                        if (!exists)
                        {
                            await awsService.UploadFileAsync(fileName, mediaStream, _cancellationTokenSource.Token);
                            Log(record.RowNumber, $"Uploaded to AWS: {fileName}");
                            return true;
                        }
                        else
                        {
                            Log(record.RowNumber, $"Skipped (Exists) AWS: {fileName}");
                            return false;
                        }
                    }
                    else
                    {
                        var azureService = CreateAzureService();
                        bool exists = await azureService.CheckIfBlobExistsAsync(fileName);

                        if (!exists)
                        {
                            string contentType = GetContentTypeFromFileName(fileName);
                            if (string.IsNullOrEmpty(contentType))
                            {
                                await azureService.UploadBlobAsync(fileName, mediaStream, null, _cancellationTokenSource.Token);
                            }
                            else
                            {
                                await azureService.UploadBlobWithContentTypeAsync(fileName, mediaStream, contentType, _cancellationTokenSource.Token);
                            }
                            Log(record.RowNumber, $"Uploaded to Azure: {fileName}");
                            return true;
                        }
                        else
                        {
                            Log(record.RowNumber, $"Skipped (Exists) Azure: {fileName}");
                            return false;
                        }
                    }
                }
                finally
                {
                    mediaStream?.Dispose();
                }
            }

            return false;
        }

        private async Task TransferM3u8Media(ExistingMediaModel record)
        {
            Log(record.RowNumber, $"Starting M3U8 transfer for: {record.media_name}");

            try
            {
                // Media name'den extension'ı çıkar (folder adı için)
                string folderName = GetFileNameWithoutExtension(record.media_name);

                // M3U8 klasörünü indir
                var m3u8Files = await _m3u8Service.DownloadM3u8FolderAsync(record.m3u8_media_url, folderName);

                Log(record.RowNumber, $"Downloaded {m3u8Files.Count} M3U8 files for: {record.media_name}");

                // Her dosyayı upload et
                foreach (var file in m3u8Files)
                {
                    if (_cancellationTokenSource.Token.IsCancellationRequested)
                        break;

                    string fullPath = $"{folderName}/{file.RelativePath}";

                    Stream fileStream = null;

                    if (file.BinaryContent != null)
                    {
                        fileStream = new MemoryStream(file.BinaryContent);
                    }
                    else if (!string.IsNullOrEmpty(file.Content))
                    {
                        fileStream = new MemoryStream(Encoding.UTF8.GetBytes(file.Content));
                    }

                    if (fileStream != null)
                    {
                        try
                        {
                            if (currentProvider == StorageProvider.AwsS3)
                            {
                                var awsService = CreateAwsService();
                                bool exists = await awsService.CheckIfFileExistsAsync(fullPath);

                                if (!exists)
                                {
                                    await awsService.UploadFileAsync(fullPath, fileStream, _cancellationTokenSource.Token);
                                    Log(record.RowNumber, $"Uploaded M3U8 to AWS: {fullPath}");
                                }
                            }
                            else
                            {
                                var azureService = CreateAzureService();
                                bool exists = await azureService.CheckIfBlobExistsAsync(fullPath);

                                if (!exists)
                                {
                                    string contentType = GetContentTypeFromFileName(fullPath);
                                    if (string.IsNullOrEmpty(contentType))
                                    {
                                        await azureService.UploadBlobAsync(fullPath, fileStream, null, _cancellationTokenSource.Token);
                                    }
                                    else
                                    {
                                        await azureService.UploadBlobWithContentTypeAsync(fullPath, fileStream, contentType, _cancellationTokenSource.Token);
                                    }
                                    Log(record.RowNumber, $"Uploaded M3U8 to Azure: {fullPath}");
                                }
                            }
                        }
                        finally
                        {
                            fileStream?.Dispose();
                        }
                    }
                }

                Log(record.RowNumber, $"Completed M3U8 transfer for: {record.media_name}");
            }
            catch (Exception ex)
            {
                Log(record.RowNumber, $"Failed M3U8 transfer for: {record.media_name} - {ex.Message}");
                throw;
            }
        }

        private string GetFileNameWithoutExtension(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return fileName;

            return Path.GetFileNameWithoutExtension(fileName);
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

            var awsService = new AwsS3StorageService(accessKey, secretKey, region, bucketName);

            if (!awsService.IsValidRegion(region))
            {
                MessageBox.Show($"Invalid AWS region: {region}. Please enter a valid region!", "Region Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!await awsService.ValidateConnectionAsync())
            {
                MessageBox.Show("Failed to connect to AWS S3 with the entered information. Please check your information!", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MediaFilterMode filterMode = MediaFilterMode.All;
            if (rbAwsLatestMedia.Checked)
                filterMode = MediaFilterMode.LatestOnly;
            else if (rbAwsHistoricalMedia.Checked)
                filterMode = MediaFilterMode.HistoricalOnly;

            await ProcessCsvAndUpload(csvPath, async (record, fileStream, fileName) =>
            {
                bool exists = await awsService.CheckIfFileExistsAsync(fileName);

                if (!exists)
                {
                    string contentType = GetContentTypeFromFileName(fileName);

                    await awsService.UploadFileAsync(fileName, fileStream, _cancellationTokenSource.Token);
                    Log(record.RowNumber, $"Uploaded: {fileName}");
                    return true;
                }
                else
                {
                    Log(record.RowNumber, $"Skipped (Exists): {fileName}");
                    return false;
                }
            }, filterMode);
        }

        private async Task TransferToAzureBlob()
        {
            string csvPath = txtCsvPath.Text;
            string blobUrl = txtAzureBlobUrl.Text;
            string sasToken = txtAzureSasToken.Text;
            string containerName = txtAzureContainerName.Text;
            string folderPath = txtAzureFolderPath.Text;

            if (!string.IsNullOrEmpty(folderPath) && !folderPath.EndsWith("/"))
            {
                folderPath += "/";
            }

            if (string.IsNullOrEmpty(csvPath) || string.IsNullOrEmpty(blobUrl) ||
       string.IsNullOrEmpty(sasToken) || string.IsNullOrEmpty(containerName))
            {
                MessageBox.Show("Please fill in all fields for Azure connection!", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var azureService = new AzureBlobStorageService(blobUrl, sasToken, containerName, folderPath);

            //if (!await azureService.ValidateConnectionAsync())
            //{
            //    MessageBox.Show("Failed to connect to Azure Blob with the entered information. Please check your information!!", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}

            MediaFilterMode filterMode = MediaFilterMode.All;
            if (rbAzureLatestMedia.Checked)
                filterMode = MediaFilterMode.LatestOnly;
            else if (rbAzureHistoricalMedia.Checked)
                filterMode = MediaFilterMode.HistoricalOnly;

            await ProcessCsvAndUpload(csvPath, async (record, fileStream, fileName) =>
            {
                bool exists = await azureService.CheckIfBlobExistsAsync(fileName);

                if (!exists)
                {
                    string contentType = GetContentTypeFromFileName(fileName);

                    if (string.IsNullOrEmpty(contentType))
                    {
                        await azureService.UploadBlobAsync(fileName, fileStream, null, _cancellationTokenSource.Token);
                    }
                    else
                    {
                        await azureService.UploadBlobWithContentTypeAsync(fileName, fileStream, contentType, _cancellationTokenSource.Token);
                    }

                    Log(record.RowNumber, $"Uploaded: {fileName}");
                    return true;
                }
                else
                {
                    Log(record.RowNumber, $"Skipped (Exists): {fileName}");
                    return false;
                }
            }, filterMode);
        }

        private async Task ProcessCsvAndUpload(string csvPath, Func<CsvRecordModel, Stream, string, Task<bool>> uploadAction, MediaFilterMode filterMode)
        {
            int uploadedCount = 0;
            int skippedCount = 0;
            int failedCount = 0;
            DateTime startTime = DateTime.Now;

            using (var stream = new FileStream(csvPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new StreamReader(stream))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Read();
                csv.ReadHeader();

                var headers = csv.Context.Reader.HeaderRecord;

                if (!headers.Contains("media_url") || !headers.Contains("product_stock_code") ||
                    !headers.Contains("media_direction") || !headers.Contains("created_date") ||
                    !headers.Contains("erp_colorCode") || !headers.Contains("integration_colorCode"))
                {
                    MessageBox.Show("The CSV format is wrong. Use the headings in the template in the same way!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var records = csv.GetRecords<dynamic>();
                var processedRecords = CsvProcessor.ProcessCsvRecords(records);
                int totalRecordsCount = processedRecords.Count;

                switch (filterMode)
                {
                    case MediaFilterMode.LatestOnly:
                        processedRecords = processedRecords.GetLatestRecords();
                        Log(0, "Processing mode: Only uploading latest media files for each product and direction.");
                        break;
                    case MediaFilterMode.HistoricalOnly:
                        processedRecords = processedRecords.GetArchiveRecords();
                        Log(0, "Processing mode: Only uploading historical (non-latest) media files.");
                        break;
                    case MediaFilterMode.All:
                    default:
                        Log(0, "Processing mode: Uploading all media files (latest and historical).");
                        break;
                }

                int filteredRecordsCount = processedRecords.Count;
                Log(0, $"Found {totalRecordsCount} total records, {filteredRecordsCount} records after applying filter.");

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
                    string colorCode = CleanString(record.color_code);

                    if (!string.IsNullOrEmpty(mediaUrl))
                    {
                        var fileStream = await DownloadFile(record.RowNumber, mediaUrl);

                        if (fileStream != null)
                        {
                            string fileExtension = Path.GetExtension(mediaUrl);
                            string fileName;

                            if (record.IsLatestRecord)
                            {
                                if (!string.IsNullOrEmpty(colorCode))
                                {
                                    fileName = $"{stockCode}_{colorCode}_{mediaDirection}{fileExtension}";
                                }
                                else
                                {
                                    fileName = $"{stockCode}_0_{mediaDirection}{fileExtension}";
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(colorCode))
                                {
                                    fileName = $"_{stockCode}_{colorCode}_{mediaDirection}_{Guid.NewGuid()}{fileExtension}";
                                }
                                else
                                {
                                    fileName = $"_{stockCode}_0_{mediaDirection}_{Guid.NewGuid()}{fileExtension}";
                                }
                            }

                            try
                            {
                                bool wasUploaded = await uploadAction(record, fileStream, fileName);

                                if (wasUploaded)
                                    uploadedCount++;
                                else
                                    skippedCount++;
                            }
                            catch (Exception ex)
                            {
                                Log(record.RowNumber, $"Failed to upload: {fileName} - Error: {ex.Message}");
                                failedCount++;
                            }
                        }
                        else
                        {
                            failedCount++;
                        }
                    }
                }
            }

            TimeSpan duration = DateTime.Now - startTime;
            string formattedDuration = duration.ToString(@"hh\:mm\:ss");

            Log(0, $"--------------------------------------------------");
            Log(0, $"TRANSFER COMPLETED");
            Log(0, $"Duration: {formattedDuration}");
            Log(0, $"Total Files Processed: {uploadedCount + skippedCount + failedCount}");
            Log(0, $"Successfully Uploaded: {uploadedCount}");
            Log(0, $"Skipped (Already Exists): {skippedCount}");
            if (failedCount > 0)
                Log(0, $"Failed: {failedCount}");
            Log(0, $"--------------------------------------------------");

            MessageBox.Show(
                $"Transfer completed!\n\nUploaded: {uploadedCount}\nSkipped: {skippedCount}\nFailed: {failedCount}",
                "Transfer Results",
                MessageBoxButtons.OK,
                failedCount > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);
        }

        private AwsS3StorageService CreateAwsService()
        {
            return new AwsS3StorageService(
                txtAccessKey.Text,
                txtSecretKey.Text,
                txtRegion.Text,
                txtBucketName.Text
            );
        }

        private AzureBlobStorageService CreateAzureService()
        {
            return new AzureBlobStorageService(
                txtAzureBlobUrl.Text,
                txtAzureSasToken.Text,
                txtAzureContainerName.Text,
                txtAzureFolderPath.Text
            );
        }

        private string CleanString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return Regex.Replace(input, @"[^a-zA-Z0-9]", "");
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
            string template = "";

            if (currentTransferMode == DataTransferMode.CreateWithNewList)
            {
                template = Properties.Resources.template;
            }
            else
            {
                template = "media_name,media_extension,media_url,is_converted_m3u8,m3u8_media_url\n" +
                          "example_media,jpg,https://example.com/image.jpg,false,\n" +
                          "example_video,mp4,data:video/mp4;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mP8/5+hHgAHggJ/PchI7wAAAABJRU5ErkJggg==,true,https://example.com/video/video.m3u8";
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
                saveFileDialog.Title = "Save Template File";
                saveFileDialog.FileName = "template.csv";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        byte[] fileContent = Encoding.UTF8.GetBytes(template);
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

        private void rbAwsS3_CheckedChanged(object sender, EventArgs e)
        {
            btnSelectProvider.Enabled = rbAwsS3.Checked || rbAzureBlob.Checked;
        }

        private void rbAzureBlob_CheckedChanged(object sender, EventArgs e)
        {
            btnSelectProvider.Enabled = rbAwsS3.Checked || rbAzureBlob.Checked;
        }

        private void btnSelectProvider_Click(object sender, EventArgs e)
        {
            tabControl.TabPages.Clear();

            tabControl.TabPages.Add(tabPageProviderSelection);

            if (rbAwsS3.Checked)
            {
                currentProvider = StorageProvider.AwsS3;

                if (!tabControl.TabPages.Contains(tabPageProviderSelection))
                    tabControl.TabPages.Add(tabPageProviderSelection);

                if (!tabControl.TabPages.Contains(tabPageAWS))
                    tabControl.TabPages.Add(tabPageAWS);

                if (!tabControl.TabPages.Contains(tabPageFile))
                    tabControl.TabPages.Add(tabPageFile);

                if (!tabControl.TabPages.Contains(tabPageTransfer))
                    tabControl.TabPages.Add(tabPageTransfer);

                tabControl.SelectedTab = tabPageAWS;
            }
            else if (rbAzureBlob.Checked)
            {
                currentProvider = StorageProvider.AzureBlob;

                if (!tabControl.TabPages.Contains(tabPageProviderSelection))
                    tabControl.TabPages.Add(tabPageProviderSelection);

                if (!tabControl.TabPages.Contains(tabPageAzure))
                    tabControl.TabPages.Add(tabPageAzure);

                if (!tabControl.TabPages.Contains(tabPageFile))
                    tabControl.TabPages.Add(tabPageFile);

                if (!tabControl.TabPages.Contains(tabPageTransfer))
                    tabControl.TabPages.Add(tabPageTransfer);

                tabControl.SelectedTab = tabPageAzure;
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
                if (keys.ContainsKey("blob_url") && keys.ContainsKey("sas_token") && keys.ContainsKey("container_name"))
                {
                    txtAzureBlobUrl.Text = keys["blob_url"];
                    txtAzureSasToken.Text = keys["sas_token"];
                    txtAzureContainerName.Text = keys["container_name"];

                    if (keys.ContainsKey("folder_path"))
                    {
                        txtAzureFolderPath.Text = keys["folder_path"];
                    }

                    return true;
                }
                return false;
            }, "Azure Blob");
        }

        private void btnAzureKeysCsvTemplateDownload_Click(object sender, EventArgs e)
        {
            string azureTemplate = "blob_url,sas_token,container_name,folder_path\n";
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

        private string GetContentTypeFromFileName(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower();

            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                case ".bmp":
                    return "image/bmp";
                case ".mp4":
                    return "video/mp4";
                case ".avi":
                    return "video/x-msvideo";
                case ".mov":
                    return "video/quicktime";
                case ".pdf":
                    return "application/pdf";
                case ".m3u8":
                    return "application/vnd.apple.mpegurl";
                case ".ts":
                    return "video/mp2t";
                default:
                    return "application/octet-stream";
            }
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            rtbLogs.Clear();
        }
    }
}
