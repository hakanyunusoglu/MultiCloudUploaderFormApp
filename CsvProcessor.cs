using System;
using System.Collections.Generic;
using System.Linq;

namespace MediaCloudUploaderFormApp
{
    public static class CsvProcessor
    {
        public static List<CsvRecordModel> ProcessCsvRecords(IEnumerable<dynamic> records)
        {
            var processedRecords = records.Select((r, index) =>
            {
                string erpColorCode = !string.IsNullOrEmpty(r.erp_colorCode?.ToString()) ? FormatColorCode(r.erp_colorCode.ToString()) : null;

                string integrationColorCode = !string.IsNullOrEmpty(r.integration_colorCode?.ToString()) ? FormatColorCode(r.integration_colorCode.ToString()) : null;

                return new CsvRecordModel
                {
                    RowNumber = index + 1,
                    product_stock_code = r.product_stock_code,
                    media_direction = r.media_direction,
                    created_date = DateTime.Parse(r.created_date.ToString()),
                    media_url = r.media_url,
                    erp_colorCode = erpColorCode,
                    integration_colorCode = integrationColorCode,
                    color_code = !string.IsNullOrEmpty(integrationColorCode) ? integrationColorCode : !string.IsNullOrEmpty(erpColorCode) ? erpColorCode : null,
                    IsLatestRecord = false
                };
            }).ToList();

            var groupedByStockCode = processedRecords.GroupBy(r => r.product_stock_code);

            foreach (var stockCodeGroup in groupedByStockCode)
            {
                var groupedByColorCode = stockCodeGroup.Where(x => !string.IsNullOrEmpty(x.color_code)).GroupBy(r => r.color_code);

                foreach (var colorCodeGroup in groupedByColorCode)
                {
                    var groupedByDirection = colorCodeGroup.GroupBy(r => r.media_direction);

                    foreach (var directionGroup in groupedByDirection)
                    {
                        var latestRecord = directionGroup.OrderByDescending(r => r.created_date).First();
                        latestRecord.IsLatestRecord = true;
                    }
                }
            }

            return processedRecords;
        }

        public static List<ExistingMediaModel> ProcessCsvRecordsToExistingMedias(IEnumerable<dynamic> records)
        {
            var processedRecords = records.Select((r, index) =>
            {
                return new ExistingMediaModel
                {
                    RowNumber = index + 1,
                    media_name = r.media_name?.ToString() ?? "",
                    media_extension = r.media_extension?.ToString() ?? "",
                    media_url = r.media_url?.ToString() ?? "",
                    is_converted_m3u8 = ParseBooleanValue(r.is_converted_m3u8?.ToString()),
                    m3u8_media_url = r.m3u8_media_url?.ToString() ?? ""
                };
            }).ToList();

            return processedRecords;
        }

        private static bool ParseBooleanValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            string cleanValue = value.Trim().ToLower();

            switch (cleanValue)
            {
                case "true":
                case "1":
                case "yes":
                case "y":
                case "DOĞRU":
                case "doğru":
                    return true;
                case "false":
                case "yanlış":
                case "YANLIŞ":
                case "0":
                case "no":
                case "n":
                case "":
                    return false;
                default:
                    if (bool.TryParse(cleanValue, out bool result))
                        return result;

                    return false;
            }
        }

        public static List<CsvRecordModel> GetLatestRecords(this IEnumerable<CsvRecordModel> records)
        {
            return records.Where(r => r.IsLatestRecord).ToList();
        }

        public static List<CsvRecordModel> GetArchiveRecords(this IEnumerable<CsvRecordModel> records)
        {
            return records.Where(r => !r.IsLatestRecord).ToList();
        }

        private static string FormatColorCode(string colorCode)
        {
            if (!string.IsNullOrEmpty(colorCode) && colorCode.Length < 3)
            {
                return colorCode.PadLeft(3, '0');
            }
            return colorCode;
        }
    }
}
