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

        internal void UploadFromStream(string accountName, string accountKey, string containerName, string fileName, string sourceFileName, string fileContentType, System.IO.Stream fileInputStream) { //, string name, string fileDescription) {

            Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount =
               Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(
                   string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1};BlobEndpoint=https://{0}.blob.core.windows.net/", accountName, accountKey)
                  );
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = blobClient.GetContainerReference(containerName);

            string ext = System.IO.Path.GetExtension(sourceFileName);
            //string fileName = String.Format(

            Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
            blockBlob.Properties.ContentType = fileContentType;
            //blockBlob.Metadata.Add("name", name);
            blockBlob.Metadata.Add("originalfilename", sourceFileName);
            //blockBlob.Metadata.Add("userid", userId.ToString());
            //blockBlob.Metadata.Add("ownerid", userId.ToString());
            DateTime created = DateTime.UtcNow;
            // https://msdn.microsoft.com/en-us/library/8kb3ddd4(v=vs.110).aspx
            // http://stackoverflow.com/questions/114983/given-a-datetime-object-how-do-i-get-a-iso-8601-date-in-string-format
            //blockBlob.Metadata.Add("username", userName);
            blockBlob.Metadata.Add("created", created.ToString("yyyy-MM-ddTHH:mm:ss")); // "yyyy-MM-ddTHH:mm:ssZ"
            blockBlob.Metadata.Add("modified", created.ToString("yyyy-MM-ddTHH:mm:ss")); // "yyyy-MM-ddTHH:mm:ssZ"
            blockBlob.Metadata.Add("fileext", ext);

            blockBlob.UploadFromStream(fileInputStream);

            blockBlob.SetMetadata();

        }



        internal void UploadFromString(string accountName, string accountKey, string containerName, string fileName, string sourceFileName, string fileContentType, string content) { //, string name, string fileDescription) {

            Microsoft.WindowsAzure.Storage.CloudStorageAccount storageAccount =
               Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(
                   string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1};BlobEndpoint=https://{0}.blob.core.windows.net/", accountName, accountKey)
                  );
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            Microsoft.WindowsAzure.Storage.Blob.CloudBlobContainer container = blobClient.GetContainerReference(containerName);
            container.CreateIfNotExists();
            string ext = System.IO.Path.GetExtension(sourceFileName);
            //string fileName = String.Format(

            Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);
            blockBlob.Properties.ContentType = fileContentType;
            //blockBlob.Metadata.Add("name", name);
            blockBlob.Metadata.Add("originalfilename", sourceFileName);
            //blockBlob.Metadata.Add("userid", userId.ToString());
            //blockBlob.Metadata.Add("ownerid", userId.ToString());
            DateTime created = DateTime.UtcNow;
            // https://msdn.microsoft.com/en-us/library/8kb3ddd4(v=vs.110).aspx
            // http://stackoverflow.com/questions/114983/given-a-datetime-object-how-do-i-get-a-iso-8601-date-in-string-format
            //blockBlob.Metadata.Add("username", userName);
            blockBlob.Metadata.Add("created", created.ToString("yyyy-MM-ddTHH:mm:ss")); // "yyyy-MM-ddTHH:mm:ssZ"
            blockBlob.Metadata.Add("modified", created.ToString("yyyy-MM-ddTHH:mm:ss")); // "yyyy-MM-ddTHH:mm:ssZ"
            blockBlob.Metadata.Add("fileext", ext);

            blockBlob.UploadText(content, Encoding.UTF8); // .UploadFromStream(fileInputStream);

            blockBlob.SetMetadata();

        }
    }
}