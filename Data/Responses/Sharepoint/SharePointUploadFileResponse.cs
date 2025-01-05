namespace PrettyNeatGenericAPI.Data.Responses.Sharepoint
{
    public class SharePointUploadFileResponse
    {
        public string CheckInComment { get; set; }
        public int CheckOutType { get; set; }
        public string ContentTag { get; set; }
        public int CustomizedPageStatus { get; set; }
        public string ETag { get; set; }
        public bool Exists { get; set; }
        public bool IrmEnabled { get; set; }
        public string Length { get; set; }
        public int Level { get; set; }
        public string LinkingUri { get; set; }
        public string LinkingUrl { get; set; }
        public int MajorVersion { get; set; }
        public int MinorVersion { get; set; }
        public string Name { get; set; }
        public string ServerRelativeUrl { get; set; }
        public string TimeCreated { get; set; }
        public string TimeLastModified { get; set; }
        public string Title { get; set; }
        public int UIVersion { get; set; }
        public string UIVersionLabel { get; set; }
        public string UniqueId { get; set; }
    }
}
