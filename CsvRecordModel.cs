using System;

namespace MediaCloudUploaderFormApp
{
    public class CsvRecordModel
    {
        public string product_stock_code { get; set; }
        public string media_direction { get; set; }
        public DateTime created_date { get; set; }
        public string media_url { get; set; }
        public string erp_colorCode { get; set; }
        public string integration_colorCode { get; set; }
        public string color_code { get; set; }
        public bool IsLatestRecord { get; set; }
        public int RowNumber { get; set; }
    }
}
