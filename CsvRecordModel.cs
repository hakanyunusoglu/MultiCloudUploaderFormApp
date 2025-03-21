using System;

namespace TransferMediaCsvToS3App
{
    public class CsvRecordModel
    {
        public string product_stock_code { get; set; }
        public string media_direction { get; set; }
        public DateTime created_date { get; set; }
        public string media_url { get; set; }
        public bool IsLatestRecord { get; set; }
        public int RowNumber { get; set; }
    }
}
