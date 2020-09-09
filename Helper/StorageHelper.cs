using System;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Threading.Tasks;

namespace varprime.app365
{

    public partial class StorageHelper
    {

        string accessKey;
        string accountName;
        string connectionString;
        CloudStorageAccount storageAccount;
        ConnectorConfig config;

        public StorageHelper(ConnectorConfig config)
        {
            this.config = config;
            this.accountName = config.StorageAccountName;
            this.accessKey = config.StorageAccountKey;
            this.connectionString = config.StorageEndpoint;
            storageAccount = CloudStorageAccount.Parse(connectionString);
        }

        public async Task<Boolean> Upload(MemoryStream stream, string itemId)
        {
            CloudBlockBlob blob;
            CloudBlobClient client;
            CloudBlobContainer container;

            client = storageAccount.CreateCloudBlobClient();

            container = client.GetContainerReference("pictures");

            await container.CreateIfNotExistsAsync();

            blob = container.GetBlockBlobReference(String.Format("{0}.jpg", itemId));
            blob.Properties.ContentType = "application/jpeg";

            await blob.UploadFromStreamAsync(stream);

            return true;
        }

        public async Task<MemoryStream> Download(string itemId)
        {
            MemoryStream stream = new MemoryStream();
            CloudBlockBlob blob;
            CloudBlobClient client;
            CloudBlobContainer container;

            try
            {
                client = storageAccount.CreateCloudBlobClient();

                container = client.GetContainerReference("pictures");

                blob = container.GetBlockBlobReference(String.Format("{0}.jpg", itemId));

                await blob.DownloadToStreamAsync(stream);
                
                return stream;

            }
            catch (Exception e)
            {
                return null;
            }

        }
    }
}
