using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Me.One.Core.AzureBlob
{
    public class AzureBlobStorage : ICloudStorage
    {
        private readonly string _connectionString;
        private readonly string _containerName;
        private readonly int _maxEvents;
        private readonly AzureBlobSettings _settings;
        private bool _isInitConnection;
        private CloudStorageAccount _storageAccount;
        private CloudBlobClient CloudBlobClient;
        private CloudBlobContainer CloudBlobContainer;

        public AzureBlobStorage(string connectionString, string containerName)
        {
            _containerName = containerName;
            _connectionString = connectionString;
            InitConnection();
            _maxEvents = 1000;
        }

        public AzureBlobStorage(AzureBlobSettings settings)
        {
            RootPath = settings.RootPath;
            LowerCasePath = settings.LowerCasePath;
            _containerName = settings.ContainerName;
            _connectionString = settings.StorageConnection;
            InitConnection();
            _maxEvents = 1000;
        }

        public AzureBlobStorage(IConfigurationSection configSection)
        {
            _settings = configSection.Get<AzureBlobSettings>();
            RootPath = _settings.RootPath;
            LowerCasePath = _settings.LowerCasePath;
            _containerName = _settings.ContainerName;
            _connectionString = _settings.StorageConnection;
            InitConnection();
        }

        private string RootPath { get; }

        private bool LowerCasePath { get; }

        private CloudBlobClient _cloudBlobClient
        {
            get
            {
                if (CloudBlobClient == null)
                    CloudBlobClient = _storageAccount.CreateCloudBlobClient();
                return CloudBlobClient;
            }
        }

        private CloudBlobContainer _cloudBlobContainer
        {
            get
            {
                if (CloudBlobContainer == null)
                    CloudBlobContainer = _cloudBlobClient.GetContainerReference(_containerName);
                return CloudBlobContainer;
            }
        }

        public async Task UploadAsync(string localPath, string cloudPath, string mime = null)
        {
            if (!InitConnection())
                return;
            cloudPath = StandardizedKey(cloudPath);
            var blockBlobReference = _cloudBlobContainer.GetBlockBlobReference(cloudPath);
            if (!string.IsNullOrEmpty(mime))
                blockBlobReference.Properties.ContentType = mime;
            await blockBlobReference.UploadFromFileAsync(localPath);
        }

        public async Task<string> GetSignedUrl(string key, DateTime? expiresOn = null)
        {
            InitConnection();
            key = StandardizedKey(key);
            var blockBlobReference = _cloudBlobContainer.GetBlockBlobReference(key);
            if (!expiresOn.HasValue)
                return blockBlobReference.Uri.ToString();
            var policy = new SharedAccessBlobPolicy
            {
                SharedAccessStartTime = DateTimeOffset.UtcNow.AddMinutes(-5.0),
                SharedAccessExpiryTime = DateTime.SpecifyKind(expiresOn.Value, DateTimeKind.Utc),
                Permissions = SharedAccessBlobPermissions.Read
            };
            var sharedAccessSignature = blockBlobReference.GetSharedAccessSignature(policy);
            return blockBlobReference.Uri + sharedAccessSignature;
        }

        public async Task<string> GetContainerSas(DateTime? expiresOn = null)
        {
            InitConnection();
            if (!expiresOn.HasValue)
                expiresOn = DateTime.Now.AddHours(1.0);
            return _cloudBlobContainer.GetSharedAccessSignature(new SharedAccessBlobPolicy
            {
                SharedAccessStartTime = DateTimeOffset.UtcNow.AddMinutes(-5.0),
                SharedAccessExpiryTime = DateTime.SpecifyKind(expiresOn.Value, DateTimeKind.Utc),
                Permissions = SharedAccessBlobPermissions.Read
            });
        }

        public async Task UploadAsync(string cloudPath, byte[] buffer, string mime = null)
        {
            if (!InitConnection())
                return;
            cloudPath = StandardizedKey(cloudPath);
            var blockBlobReference = _cloudBlobContainer.GetBlockBlobReference(cloudPath);
            if (!string.IsNullOrEmpty(mime))
                blockBlobReference.Properties.ContentType = mime;
            await blockBlobReference.UploadFromByteArrayAsync(buffer, 0, buffer.Length);
        }

        public async Task UploadAsync(string cloudPath, Stream buffer, string mime = null)
        {
            if (!InitConnection())
                return;
            cloudPath = StandardizedKey(cloudPath);
            var blockBlobReference = _cloudBlobContainer.GetBlockBlobReference(cloudPath);
            if (!string.IsNullOrEmpty(mime))
                blockBlobReference.Properties.ContentType = mime;
            await blockBlobReference.UploadFromStreamAsync(buffer);
        }

        public async Task<string> UploadBase64(string cloudPath, string base64, string mime = null)
        {
            if (!InitConnection())
                return null;
            cloudPath = StandardizedKey(cloudPath);
            var blob = _cloudBlobContainer.GetBlockBlobReference(cloudPath);
            if (!string.IsNullOrEmpty(mime))
                blob.Properties.ContentType = mime;
            using (var stream = new MemoryStream(Convert.FromBase64String(base64)))
            {
                await blob.UploadFromStreamAsync(stream);
                return blob.Uri.AbsoluteUri;
            }
        }

        public async Task DownloadAsync(string cloudPath, string destinationPath)
        {
            if (!InitConnection())
                return;
            cloudPath = StandardizedKey(cloudPath);
            await _cloudBlobContainer.GetBlockBlobReference(cloudPath)
                .DownloadToFileAsync(destinationPath, FileMode.Create);
        }

        public async Task<string> DownloadTextAsync(string cloudPath)
        {
            if (!InitConnection())
                return null;
            cloudPath = StandardizedKey(cloudPath);
            return await _cloudBlobContainer.GetBlockBlobReference(cloudPath).DownloadTextAsync();
        }

        public async Task UploadFolderAsync(
            string cloudFolder,
            string localFolder,
            IEnumerable<string> fileNames = null,
            string mime = null)
        {
            var dirInfo = new DirectoryInfo(localFolder);
            var fileInfoArray1 = dirInfo.GetFiles("*.*", SearchOption.AllDirectories);
            if (fileNames != null)
                fileInfoArray1 = fileInfoArray1.Where((Func<FileInfo, bool>) (f => fileNames.Contains(f.Name)))
                    .ToArray();
            var fileInfoArray = fileInfoArray1;
            for (var index = 0; index < fileInfoArray.Length; ++index)
            {
                var fileInfo = fileInfoArray[index];
                var cloudPath = StandardizedKey(Path.Combine(cloudFolder,
                    BuildCloudPath(dirInfo, fileInfo.Directory, fileInfo.Name)));
                await UploadAsync(fileInfo.FullName, cloudPath, mime);
            }

            fileInfoArray = null;
            dirInfo = null;
        }

        public async Task DownloadFolderAsync(
            string cloudFolder,
            string localFolder,
            IEnumerable<string> fileNames = null)
        {
            if (!InitConnection())
                return;
            cloudFolder = StandardizedKey(cloudFolder);
            await DownloadFolder(_cloudBlobContainer.GetDirectoryReference(cloudFolder), new DirectoryInfo(localFolder),
                fileNames);
        }

        public async Task DeleteFolderAsync(string cloudFolder, IEnumerable<string> fileNames = null)
        {
            if (!InitConnection())
                return;
            if (fileNames != null)
                foreach (var fileName in fileNames)
                {
                    Path.Combine(cloudFolder, fileName);
                    var num = await _cloudBlobContainer.GetBlockBlobReference(cloudFolder).DeleteIfExistsAsync()
                        ? 1
                        : 0;
                }

            cloudFolder = StandardizedKey(cloudFolder);
            await DeleteFolder(_cloudBlobContainer.GetDirectoryReference(cloudFolder), fileNames);
        }

        public async Task DeleteAsync(string cloudPath)
        {
            if (!InitConnection())
                return;
            cloudPath = StandardizedKey(cloudPath);
            var num = await _cloudBlobContainer.GetBlockBlobReference(cloudPath).DeleteIfExistsAsync() ? 1 : 0;
        }

        private bool InitConnection()
        {
            if (!_isInitConnection && CloudStorageAccount.TryParse(_connectionString, out _storageAccount))
                _isInitConnection = true;
            return _isInitConnection;
        }

        private async Task DownloadFolder(
            CloudBlobDirectory blobDirectory,
            DirectoryInfo localDir,
            IEnumerable<string> fileNames)
        {
            if (!localDir.Exists)
                localDir.Create();
            BlobContinuationToken dirToken = null;
            do
            {
                var blobResultSegment = await blobDirectory.ListBlobsSegmentedAsync(dirToken);
                dirToken = blobResultSegment.ContinuationToken;
                foreach (var result in blobResultSegment.Results)
                {
                    var name = GetName(result);
                    switch (result)
                    {
                        case CloudBlobDirectory _:
                            var blobDirectory1 = result as CloudBlobDirectory;
                            var directoryInfo = new DirectoryInfo(Path.Combine(localDir.FullName, name));
                            await DownloadFolder(blobDirectory1, localDir, fileNames);
                            continue;
                        case CloudBlockBlob _ when fileNames != null && fileNames.Contains(name):
                            await (result as CloudBlockBlob).DownloadToFileAsync(Path.Combine(localDir.FullName, name),
                                FileMode.Create);
                            continue;
                        case CloudBlockBlob _ when fileNames == null:
                            await (result as CloudBlockBlob).DownloadToFileAsync(Path.Combine(localDir.FullName, name),
                                FileMode.Create);
                            continue;
                        default:
                            continue;
                    }
                }
            } while (dirToken != null);

            dirToken = null;
        }

        private async Task DeleteFolder(
            CloudBlobDirectory cloudBlobDirectory,
            IEnumerable<string> fileNames = null)
        {
            BlobContinuationToken dirToken = null;
            do
            {
                var blobResultSegment = await cloudBlobDirectory.ListBlobsSegmentedAsync(dirToken);
                dirToken = blobResultSegment.ContinuationToken;
                foreach (var result in blobResultSegment.Results)
                {
                    var name = GetName(result);
                    switch (result)
                    {
                        case CloudBlobDirectory _:
                            await DeleteFolder(result as CloudBlobDirectory, fileNames);
                            continue;
                        case CloudBlockBlob _ when fileNames != null && fileNames.Contains(name):
                            var num1 = await (result as CloudBlockBlob).DeleteIfExistsAsync() ? 1 : 0;
                            continue;
                        case CloudBlockBlob _ when !string.IsNullOrEmpty(name):
                            var num2 = await (result as CloudBlockBlob).DeleteIfExistsAsync() ? 1 : 0;
                            continue;
                        default:
                            continue;
                    }
                }
            } while (dirToken != null);

            dirToken = null;
        }

        private string BuildCloudPath(DirectoryInfo rootDirectory, DirectoryInfo info, string path)
        {
            if (info.FullName == rootDirectory.FullName)
                return path;
            var path1 = Path.Combine(info.Name, path);
            return BuildCloudPath(rootDirectory, info.Parent, path1);
        }

        private string GetName(IListBlobItem item)
        {
            return item.Uri.Segments.Last();
        }

        private string StandardizedKey(string key)
        {
            if (LowerCasePath)
                key = key.ToLower();
            key = key.Replace('\\', '/').Trim('/');
            if (!string.IsNullOrEmpty(RootPath))
                key = RootPath + "/" + key;
            return key;
        }
    }
}