using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Sistrategia.SAT.CFDiWebSite.CloudStorage
{
    public class CloudStorageMananger
    {
        public string GetTempUrl(string accountName, string accountKey, string fullPath) {
            Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount =
               Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(
                   string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1};BlobEndpoint=https://{0}.blob.core.windows.net/", accountName, accountKey)
                  );

            Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            var blob = blobClient.GetBlobReferenceFromServer(new Uri(fullPath));

            var readPolicy = new Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPolicy() {
                Permissions = Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPermissions.Read, // SharedAccessPermissions.Read,
                SharedAccessExpiryTime = DateTime.UtcNow + TimeSpan.FromMinutes(10)
            };

            string resultUrl = new Uri(blob.Uri.AbsoluteUri + blob.GetSharedAccessSignature(readPolicy)).ToString();

            return resultUrl;
        }

        public string GetTempDownloadUrl(string accountName, string accountKey, string fullPath) {
            Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount =
               Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(
                   string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1};BlobEndpoint=https://{0}.blob.core.windows.net/", accountName, accountKey)
                  );
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            var blob = blobClient.GetBlobReferenceFromServer(new Uri(fullPath));

            var readPolicy = new Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPolicy() {
                Permissions = Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPermissions.Read, // SharedAccessPermissions.Read,
                SharedAccessExpiryTime = DateTime.UtcNow + TimeSpan.FromMinutes(10)
            };

            string resultUrl = new Uri(blob.Uri.AbsoluteUri + blob.GetSharedAccessSignature(readPolicy,
                    new SharedAccessBlobHeaders {
                        ContentDisposition = blob.Metadata.ContainsKey("originalfilename") ? "attachment; filename=" + blob.Metadata["originalfilename"] : "attachment; filename=FileUnknown",
                        ContentType = blob.Properties.ContentType
                    }
                    )).ToString();

            return resultUrl;
        }
    }
}