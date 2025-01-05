namespace PrettyNeatGenericAPI.Data.Responses.Sharepoint
{
    public class SharepointCreateFolderResponse
    {
        public InternalCreateFolderResponse d { get; set; }
    }
    public partial class InternalCreateFolderResponse
    {
        public bool Exists { get; set; }
        public bool IsWOPIEnabled { get; set; }
        public int ItemCount { get; set; }
        public string Name { get; set; }
        public int ProgID { get; set; }
        public string ServerRelativeUrl { get; set; }
        public DateTimeOffset TimeCreated { get; set; }
        public DateTimeOffset TimeLastModified { get; set; }
        public string UniqueId { get; set; }
        public string WelcomePage { get; set; }
    }
}
