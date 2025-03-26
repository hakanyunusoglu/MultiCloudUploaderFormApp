using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MediaCloudUploaderFormApp
{
    public class AwsS3StorageService
    {
        private readonly string _accessKey;
        private readonly string _secretKey;
        private readonly string _region;
        private readonly string _bucketName;
        private readonly AmazonS3Client _s3Client;

        public AwsS3StorageService(string accessKey, string secretKey, string region, string bucketName)
        {
            _accessKey = accessKey;
            _secretKey = secretKey;
            _region = region;
            _bucketName = bucketName;

            _s3Client = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.GetBySystemName(region));
        }

        public bool IsValidRegion(string regionName)
        {
            return RegionEndpoint.EnumerableAllRegions.Any(region =>
                region.SystemName.Equals(regionName, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<bool> ValidateConnectionAsync()
        {
            try
            {
                var response = await _s3Client.ListBucketsAsync();
                if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
                {
                    return false;
                }

                if (!response.Buckets.Any(b => b.BucketName == _bucketName))
                {
                    return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> CheckIfFileExistsAsync(string fileName)
        {
            try
            {
                var request = new ListObjectsV2Request
                {
                    BucketName = _bucketName,
                    Prefix = fileName
                };

                var response = await _s3Client.ListObjectsV2Async(request);
                return response.S3Objects.Any(obj => obj.Key == fileName);
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task UploadFileAsync(string fileName, Stream content, CancellationToken cancellationToken = default)
        {
            try
            {
                var request = new PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileName,
                    InputStream = content
                };

                await _s3Client.PutObjectAsync(request, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error uploading file to S3: {ex.Message}", ex);
            }
        }

        public string GetFileUrl(string fileName)
        {
            try
            {
                return $"https://{_bucketName}.s3.{_region}.amazonaws.com/{fileName}";
            }
            catch (Exception ex)
            {
                throw new Exception($"Error generating S3 URL: {ex.Message}", ex);
            }
        }
    }
}