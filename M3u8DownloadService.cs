using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MediaCloudUploaderFormApp
{
    public class M3u8DownloadService
    {
        private readonly HttpClient _httpClient;

        public M3u8DownloadService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<M3u8FileInfo>> DownloadM3u8FolderAsync(string m3u8Url, string mediaName)
        {
            var files = new List<M3u8FileInfo>();

            // Base URL'i al (son / karakterine kadar)
            var baseUrl = m3u8Url.Substring(0, m3u8Url.LastIndexOf('/') + 1);

            try
            {
                // Ana m3u8 dosyasını indir
                var mainM3u8Content = await _httpClient.GetStringAsync(m3u8Url);
                files.Add(new M3u8FileInfo
                {
                    RelativePath = $"{mediaName}.m3u8",
                    Content = mainM3u8Content,
                    IsDirectory = false
                });

                // master.m3u8 dosyasını kontrol et ve indir
                var masterUrl = baseUrl + "master.m3u8";
                try
                {
                    var masterContent = await _httpClient.GetStringAsync(masterUrl);
                    files.Add(new M3u8FileInfo
                    {
                        RelativePath = "master.m3u8",
                        Content = masterContent,
                        IsDirectory = false
                    });
                }
                catch
                {
                    // master.m3u8 yoksa devam et
                }

                // Resolution klasörlerini bul
                var resolutionFolders = ExtractResolutionFolders(mainM3u8Content);

                foreach (var folder in resolutionFolders)
                {
                    var folderUrl = baseUrl + folder + "/";

                    // index.m3u8 dosyasını indir
                    var indexUrl = folderUrl + "index.m3u8";
                    try
                    {
                        var indexContent = await _httpClient.GetStringAsync(indexUrl);
                        files.Add(new M3u8FileInfo
                        {
                            RelativePath = $"{folder}/index.m3u8",
                            Content = indexContent,
                            IsDirectory = false
                        });

                        // Segment dosyalarını bul ve indir
                        var segmentFiles = ExtractSegmentFiles(indexContent);
                        foreach (var segment in segmentFiles)
                        {
                            var segmentUrl = folderUrl + segment;
                            try
                            {
                                var segmentData = await _httpClient.GetByteArrayAsync(segmentUrl);
                                files.Add(new M3u8FileInfo
                                {
                                    RelativePath = $"{folder}/{segment}",
                                    BinaryContent = segmentData,
                                    IsDirectory = false
                                });
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Segment indirilemedi: {segment} - {ex.Message}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Resolution klasörü indirilemedi: {folder} - {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"M3U8 klasörü indirilemedi: {ex.Message}", ex);
            }

            return files;
        }

        private List<string> ExtractResolutionFolders(string m3u8Content)
        {
            var folders = new List<string>();
            var lines = m3u8Content.Split('\n');

            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();
                if (!trimmedLine.StartsWith("#") && !string.IsNullOrEmpty(trimmedLine))
                {
                    // Eğer satır bir klasör referansı içeriyorsa (örn: 720p/index.m3u8)
                    if (trimmedLine.Contains("/"))
                    {
                        var folderName = trimmedLine.Split('/')[0];
                        if (!folders.Contains(folderName))
                        {
                            folders.Add(folderName);
                        }
                    }
                }
            }

            // Eğer hiç klasör bulunamadıysa, yaygın resolution klasörlerini dene
            if (folders.Count == 0)
            {
                var commonResolutions = new[] { "720p", "480p", "360p", "1080p", "540p" };

                // Her resolution için kontrol et
                foreach (var resolution in commonResolutions)
                {
                    try
                    {
                        var testUrl = m3u8Content.Contains("://") ?
                            m3u8Content.Substring(0, m3u8Content.LastIndexOf('/') + 1) + resolution + "/" :
                            "";

                        if (!string.IsNullOrEmpty(testUrl))
                        {
                            folders.Add(resolution);
                        }
                    }
                    catch { }
                }
            }

            return folders;
        }

        private List<string> ExtractSegmentFiles(string indexM3u8Content)
        {
            var segments = new List<string>();
            var lines = indexM3u8Content.Split('\n');

            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();
                if (!trimmedLine.StartsWith("#") && !string.IsNullOrEmpty(trimmedLine))
                {
                    segments.Add(trimmedLine);
                }
            }

            return segments;
        }
    }
}
