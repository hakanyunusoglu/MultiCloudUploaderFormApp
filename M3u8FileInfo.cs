namespace MediaCloudUploaderFormApp
{
    public class M3u8FileInfo
    {
        public string RelativePath { get; set; }
        public string Content { get; set; }
        public byte[] BinaryContent { get; set; }
        public bool IsDirectory { get; set; }
    }
}
