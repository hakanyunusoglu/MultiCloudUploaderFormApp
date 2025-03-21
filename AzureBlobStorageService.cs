using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace TransferMediaCsvToS3App
{
    public class AzureBlobStorageService
    {
        private readonly string _connectionString;
        private readonly string _containerName;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _containerClient;

        public AzureBlobStorageService(string connectionString, string containerName)
        {
            _connectionString = connectionString;
            _containerName = containerName;

            _blobServiceClient = new BlobServiceClient(_connectionString);
            _containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        }

        public async Task<bool> ValidateConnectionAsync()
        {
            try
            {
                // Bağlantıyı kontrol etmek için container'ın var olup olmadığını kontrol ediyoruz
                await _containerClient.GetPropertiesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> CheckIfBlobExistsAsync(string blobName)
        {
            try
            {
                BlobClient blobClient = _containerClient.GetBlobClient(blobName);
                return await blobClient.ExistsAsync();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task UploadBlobAsync(string blobName, Stream content, Action<long> progressHandler = null, CancellationToken cancellationToken = default)
        {
            BlobClient blobClient = _containerClient.GetBlobClient(blobName);

            BlobUploadOptions options = new BlobUploadOptions
            {
                ProgressHandler = new Progress<long>(bytesTransferred =>
                {
                    progressHandler?.Invoke(bytesTransferred);
                }),
                TransferOptions = new Azure.Storage.StorageTransferOptions
                {
                    // Varsayılan değerler
                    InitialTransferSize = 8 * 1024 * 1024,
                    MaximumTransferSize = 4 * 1024 * 1024
                }
            };

            await blobClient.UploadAsync(content, options, cancellationToken);
        }

        public string GetBlobUrl(string blobName)
        {
            BlobClient blobClient = _containerClient.GetBlobClient(blobName);
            return blobClient.Uri.ToString();
        }
    }
}
