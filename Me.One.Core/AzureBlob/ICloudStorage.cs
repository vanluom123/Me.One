using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Me.One.Core.AzureBlob
{
    public interface ICloudStorage
    {
        Task UploadAsync(string localPath, string cloudPath, string mime = null);

        Task UploadAsync(string cloudPath, byte[] buffer, string mime = null);

        Task UploadAsync(string cloudPath, Stream buffer, string mime = null);

        Task UploadFolderAsync(
            string cloudFolder,
            string localFolder,
            IEnumerable<string> fileNames = null,
            string mime = null);

        Task DeleteFolderAsync(string cloudFolder, IEnumerable<string> fileNames);

        Task DeleteAsync(string cloudPath);

        Task DownloadAsync(string cloudPath, string destinationPath);

        Task DownloadFolderAsync(
            string cloudFolder,
            string localFolder,
            IEnumerable<string> fileNames = null);

        Task<string> DownloadTextAsync(string cloudPath);

        Task<string> UploadBase64(string cloudPath, string base64, string mime = null);

        Task<string> GetSignedUrl(string key, DateTime? expiresOn = null);

        Task<string> GetContainerSas(DateTime? expiresOn = null);
    }
}