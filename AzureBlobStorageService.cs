using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace MediaCloudUploaderFormApp
{
    public class AzureBlobStorageService
    {
        private readonly string _connectionString;
        private readonly string _containerName;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _containerClient;
        public string FolderPath { get; set; } = string.Empty;
        public AzureBlobStorageService(string connectionString, string containerName, string folderPath = "")
        {
            _connectionString = connectionString;
            _containerName = containerName;
            FolderPath = folderPath;

            _blobServiceClient = new BlobServiceClient(_connectionString);
            _containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

        }

        public AzureBlobStorageService(string blobUrl, string sasToken, string containerName, string folderPath = "")
        {
            FolderPath = folderPath;

            string fullUri;
            if (sasToken.StartsWith("?"))
            {
                fullUri = $"{blobUrl.TrimEnd('/')}/{containerName}{sasToken}";
            }
            else
            {
                fullUri = $"{blobUrl.TrimEnd('/')}/{containerName}?{sasToken.TrimStart('?')}";
            }

            Uri uri = new Uri(fullUri);

            _containerClient = new BlobContainerClient(uri);
        }

        public async Task<bool> ValidateConnectionAsync()
        {
            try
            {
                await _containerClient.GetPropertiesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> CheckIfBlobExistsAsync(string blobName)
        {
            try
            {
                string fullPath = GetFullPath(blobName);
                BlobClient blobClient = _containerClient.GetBlobClient(fullPath);
                return await blobClient.ExistsAsync();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task UploadBlobAsync(string fileName, Stream content, Action<long> progressHandler = null, CancellationToken cancellationToken = default)
        {
            string fullPath = GetFullPath(fileName);
            BlobClient blobClient = _containerClient.GetBlobClient(fullPath);

            BlobUploadOptions options = new BlobUploadOptions
            {
                ProgressHandler = new Progress<long>(bytesTransferred =>
                {
                    progressHandler?.Invoke(bytesTransferred);
                }),
                TransferOptions = new Azure.Storage.StorageTransferOptions
                {
                    InitialTransferSize = 8 * 1024 * 1024,
                    MaximumTransferSize = 4 * 1024 * 1024
                }
            };

            await blobClient.UploadAsync(content, options, cancellationToken);
        }

        public async Task UploadBlobWithContentTypeAsync(string fileName, Stream content, string contentType, CancellationToken cancellationToken = default)
        {
            string fullPath = GetFullPath(fileName);
            BlobClient blobClient = _containerClient.GetBlobClient(fullPath);

            BlobUploadOptions options = new BlobUploadOptions
            {
                HttpHeaders = new BlobHttpHeaders
                {
                    ContentType = contentType
                },
                TransferOptions = new Azure.Storage.StorageTransferOptions
                {
                    InitialTransferSize = 8 * 1024 * 1024,
                    MaximumTransferSize = 4 * 1024 * 1024
                }
            };

            await blobClient.UploadAsync(content, options, cancellationToken);
        }

        public string GetBlobUrl(string blobName)
        {
            string fullPath = GetFullPath(blobName);
            BlobClient blobClient = _containerClient.GetBlobClient(fullPath);
            return blobClient.Uri.ToString();
        }

        private string GetFullPath(string fileName)
        {
            return string.IsNullOrEmpty(FolderPath)
                ? fileName
                : Path.Combine(FolderPath.TrimEnd('/'), fileName).Replace("\\", "/");
        }
    }
}
