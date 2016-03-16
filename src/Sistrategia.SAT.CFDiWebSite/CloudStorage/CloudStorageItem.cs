using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;


namespace Sistrategia.SAT.CFDiWebSite.CloudStorage
{
    public class CloudStorageItem
    {
        public string Name { get; set; }
        public string ContentType { get; set; }
        public string Url { get; set; }
        public string ContentMD5 { get; set; }

        public string GetTempUrl(CloudStorageMananger manager) {
            return manager.GetTempUrl(ConfigurationManager.AppSettings["AzureAccountName"], ConfigurationManager.AppSettings["AzureAccountKey"], this.Url);
        }

        public string GetTempDownloadUrl(CloudStorageMananger manager) {
            return manager.GetTempDownloadUrl(ConfigurationManager.AppSettings["AzureAccountName"], ConfigurationManager.AppSettings["AzureAccountKey"], this.Url);
        }
    }
}