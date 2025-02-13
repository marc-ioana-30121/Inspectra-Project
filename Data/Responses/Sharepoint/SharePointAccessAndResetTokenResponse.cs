﻿namespace PrettyNeatGenericAPI.Data.Responses.Sharepoint
{
    public class SharePointAccessAndResetTokenResponse
    {
        public string token_type { get; set; }
        public string expires_in { get; set; }
        public string not_before { get; set; }
        public string expires_on { get; set; }
        public string resource { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
    }
}
