namespace MediaCloudUploaderFormApp
{
    public class ExistingMediaModel
    {
        public string media_name { get; set; }
        public string media_extension { get; set; }
        public string media_url { get; set; }
        public bool is_converted_m3u8 { get; set; }
        public string m3u8_media_url { get; set; }
        public int RowNumber { get; set; }
    }
}
