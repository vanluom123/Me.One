namespace Me.One.Core.AzureBlob
{
    public class AzureBlobSettings : StorageSettings
    {
        public string StorageConnection { get; set; }

        public string ContainerName { get; set; }

        public string RootPath { get; set; }

        public bool LowerCasePath { get; set; }
    }
}