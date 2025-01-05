using PrettyNeatGenericAPI.Data.Responses.Sharepoint;
using System.Net;
using System.Text.Json;
using System.Text;

namespace PrettyNeatGenericAPI.Providers
{
    public class SharePointProvider
    {
        IConfiguration _config;
        IHttpClientFactory _http;

        string ClientID = "";
        string TenantID = "";
        string ClientSecret = "";
        string ApplicationID = "";
        string TenantName = "";
        string SiteName = "";
        public SharePointProvider(IConfiguration config, IHttpClientFactory http)
        {
            _config = config;
            _http = http;

            ClientID = _config["SharePoint:ClientID"];
            TenantID = _config["SharePoint:TenantID"];
            ClientSecret = _config["SharePoint:ClientSecret"];
            ApplicationID = _config["SharePoint:ApplicationID"];
            TenantName = _config["SharePoint:TenantName"];
            SiteName = _config["SharePoint:SiteName"];
        }

        public async Task<SharePointUploadFileResponse> uploadOneOrManyFile(IFormFile file, string filePath, string fileName = "")
        {

            return await _uploadFile(file, filePath, fileName);
        }

        public async Task<SharePointUploadFileResponse> uploadOneOrManyFile(byte[] fileData, string filePath, string fileName = "")
        {

            return await _uploadFile(fileData, filePath, fileName);
        }


        public async Task<SharepointCreateFolderResponse> createOneOrManyFolders(string path)
        {

            return await _createFolder(path);
        }
        public async Task<List<SharepointCreateFolderResponse>> createOneOrManyFolders(string[] paths)
        {
            List<SharepointCreateFolderResponse> responses = new List<SharepointCreateFolderResponse>();
            List<string> pathsToList = new List<string>(paths);
            List<string> folderTemBUcker = new List<string>();
            for (int f = 0; f < pathsToList.Count; f++)
            {
                var path = pathsToList[f];
                var fullPath = folderTemBUcker.Count == 0 ? path : $"{string.Join("/", folderTemBUcker)}/{path}";
                var loopResponse = await _createFolder(fullPath);
                if (loopResponse != null)
                {
                    if (f < pathsToList.Count - 1)
                    {
                        folderTemBUcker.Add(path);
                    }
                    responses.Add(loopResponse);
                }
            }
            return responses;
        }

        public async Task<string> getFile(string filePath)
        {
            SharePointAccessAndResetTokenResponse SharePointAccessAndResetTokenResponse = await GetAccessAndResetToken();
            if (SharePointAccessAndResetTokenResponse == null)
            {
                return null;
            }

            using (var httpClient = _http.CreateClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + SharePointAccessAndResetTokenResponse.access_token);
                httpClient.DefaultRequestHeaders.Add("X-RequestDigest", "");
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json; odata=nometadata");
                var response = await httpClient.GetAsync(@$"https://{TenantName}.sharepoint.com/sites/{SiteName}/_api/web/GetFileByServerRelativeUrl('/sites/{SiteName}/Shared Documents/{filePath}')/$value");
                string getFileResponseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    return getFileResponseContent;
                }
            }
            return null;
        }

        public async Task<byte[]> getFileAsByteArray(string filePath)
        {
            SharePointAccessAndResetTokenResponse SharePointAccessAndResetTokenResponse = await GetAccessAndResetToken();
            if (SharePointAccessAndResetTokenResponse == null)
            {
                return null;
            }

            using (WebClient client = new WebClient())
            {
                client.Headers.Add("Authorization", "Bearer " + SharePointAccessAndResetTokenResponse.access_token);
                client.Headers.Add("X-RequestDigest", "");
                client.Headers.Add("Accept", "application/json; odata=nometadata");
                byte[] data = client.DownloadData(@$"https://{TenantName}.sharepoint.com/sites/{SiteName}/_api/web/GetFileByServerRelativeUrl('/sites/{SiteName}/Shared Documents/{filePath}')/$value");
                return data;
            }
            return null;
        }

        private async Task<SharePointUploadFileResponse> _uploadFile(IFormFile file, string filePath, string fileName = "")
        {
            SharePointAccessAndResetTokenResponse SharePointAccessAndResetTokenResponse = await GetAccessAndResetToken();
            if (SharePointAccessAndResetTokenResponse == null)
            {
                return null;
            }
            using (var multipartFormContent = new MultipartFormDataContent())
            {
                byte[] fileData;

                if (string.IsNullOrEmpty(fileName))
                {
                    fileName = file.FileName;
                }

                using (var br = new BinaryReader(file.OpenReadStream()))
                {
                    fileData = br.ReadBytes((int)file.OpenReadStream().Length);
                }

                ByteArrayContent fileBytes = new ByteArrayContent(fileData);

                //Add the file
                multipartFormContent.Add(fileBytes, name: "file", fileName: fileName);

                //Send it
                using (WebClient client = new WebClient())
                {
                    var path = @$"https://{TenantName}.sharepoint.com/sites/{SiteName}/_api/web/GetFolderByServerRelativeUrl('/sites/{SiteName}/Shared Documents/{filePath}')/Files/add(url='{fileName}',overwrite=true)";
                    client.Headers.Add("Authorization", "Bearer " + SharePointAccessAndResetTokenResponse.access_token);
                    client.Headers.Add("X-RequestDigest", "");
                    client.Headers.Add("Accept", "application/json; odata=nometadata");
                    byte[] response = client.UploadData(path, fileData);

                    string uploadResponseContent = Encoding.UTF8.GetString(response, 0, response.Length);
                    if (response.Any())
                    {
                        SharePointUploadFileResponse SharePointUploadFileResponse = JsonSerializer.Deserialize<SharePointUploadFileResponse>(uploadResponseContent);
                        return SharePointUploadFileResponse;
                    }
                }
            }
            return null;
        }

        private async Task<SharePointUploadFileResponse> _uploadFile(byte[] fileData, string filePath, string fileName = "")
        {
            SharePointAccessAndResetTokenResponse SharePointAccessAndResetTokenResponse = await GetAccessAndResetToken();
            if (SharePointAccessAndResetTokenResponse == null)
            {
                return null;
            }
            using (var multipartFormContent = new MultipartFormDataContent())
            {

                ByteArrayContent fileBytes = new ByteArrayContent(fileData);

                //Add the file
                multipartFormContent.Add(fileBytes, name: "file", fileName: fileName);

                //Send it
                using (WebClient client = new WebClient())
                {
                    var path = @$"https://{TenantName}.sharepoint.com/sites/{SiteName}/_api/web/GetFolderByServerRelativeUrl('/sites/{SiteName}/Shared Documents/{filePath}')/Files/add(url='{fileName}',overwrite=true)";
                    client.Headers.Add("Authorization", "Bearer " + SharePointAccessAndResetTokenResponse.access_token);
                    client.Headers.Add("X-RequestDigest", "");
                    client.Headers.Add("Accept", "application/json; odata=nometadata");
                    byte[] response = client.UploadData(path, fileData);

                    string uploadResponseContent = Encoding.UTF8.GetString(response, 0, response.Length);
                    if (response.Any())
                    {
                        SharePointUploadFileResponse SharePointUploadFileResponse = JsonSerializer.Deserialize<SharePointUploadFileResponse>(uploadResponseContent);
                        return SharePointUploadFileResponse;
                    }
                }
            }
            return null;
        }

        private async Task<SharepointCreateFolderResponse> _createFolder(string path)
        {
            SharePointAccessAndResetTokenResponse SharePointAccessAndResetTokenResponse = await GetAccessAndResetToken();
            if (SharePointAccessAndResetTokenResponse == null)
            {
                return null;
            }


            using (var httpClient = _http.CreateClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + SharePointAccessAndResetTokenResponse.access_token);
                httpClient.DefaultRequestHeaders.Add("X-RequestDigest", "");
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json; odata=nometadata");
                var response = await httpClient.PostAsync(@$"https://{TenantName}.sharepoint.com/sites/{SiteName}/_api/web/folders/add('Shared Documents/{path}')", null);
                string createFolderResponseContent = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    SharepointCreateFolderResponse sharepointCreateFolderResponse = JsonSerializer.Deserialize<SharepointCreateFolderResponse>(createFolderResponseContent);
                    return sharepointCreateFolderResponse;
                }
            }
            return null;
        }
        private async Task<SharePointAccessAndResetTokenResponse> GetAccessAndResetToken()
        {

            Dictionary<string, string> reqData = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", @$"{ClientID}@{TenantID}"},
                { "client_secret", ClientSecret },
                { "resource", @$"{ApplicationID}/{TenantName}.sharepoint.com@{TenantID}" }
            };


            using (var httpClient = _http.CreateClient())
            {
                try
                {
                    var response = await httpClient.PostAsync($"https://accounts.accesscontrol.windows.net/{TenantID}/tokens/OAuth/2", new FormUrlEncodedContent(reqData));

                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {

                        return JsonSerializer.Deserialize<SharePointAccessAndResetTokenResponse>(content);
                    }
                }
                catch (Exception ex)
                {

                    var x = ex.InnerException;
                    return null;
                }
                return null;
            }
        }
    }
}
