using System;
using System.Collections.Generic;
using System.Linq;

namespace MediaCloudUploaderFormApp
{
    public static class CsvProcessor
    {
        public static List<CsvRecordModel> ProcessCsvRecords(IEnumerable<dynamic> records)
        {
            var processedRecords = records.Select((r, index) => new CsvRecordModel
            {
                RowNumber = index + 1,
                product_stock_code = r.product_stock_code,
                media_direction = r.media_direction,
                created_date = DateTime.Parse(r.created_date.ToString()),
                media_url = r.media_url,
                color_code = r.color_code,
                IsLatestRecord = false
            }).ToList();

            var groupedByStockCode = processedRecords.GroupBy(r => r.product_stock_code);

            foreach (var stockCodeGroup in groupedByStockCode)
            {
                var groupedByDirection = stockCodeGroup.GroupBy(r => r.media_direction);

                foreach (var directionGroup in groupedByDirection)
                {
                    var latestRecord = directionGroup.OrderByDescending(r => r.created_date).First();
                    latestRecord.IsLatestRecord = true;
                }
            }

            return processedRecords;
        }

        public static List<CsvRecordModel> GetLatestRecords(this IEnumerable<CsvRecordModel> records)
        {
            return records.Where(r => r.IsLatestRecord).ToList();
        }

        public static List<CsvRecordModel> GetArchiveRecords(this IEnumerable<CsvRecordModel> records)
        {
            return records.Where(r => !r.IsLatestRecord).ToList();
        }
    }
}
