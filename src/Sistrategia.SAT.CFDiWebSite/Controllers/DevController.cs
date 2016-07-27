using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Sistrategia.SAT.CFDiWebSite.CloudStorage;
using Sistrategia.SAT.CFDiWebSite.Models;

namespace Sistrategia.SAT.CFDiWebSite.Controllers
{
    [Authorize]
    public class DevController : BaseController
    {
        // GET: Dev
        public ActionResult Index() {
            this.ViewBag.cfdiService = ConfigurationManager.AppSettings["cfdiService"];
            this.ViewBag.cfdiServiceTimeSpan = SATManager.GetCFDIServiceTimeSpan().Minutes.ToString();

            return View();
        }


        public ActionResult GetTimbradoLog() {

            CloudStorageAccount account = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["AzureDefaultStorageConnectionString"]);
            CloudBlobClient client = account.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference(ConfigurationManager.AppSettings["AzureDefaultStorage"]);

            var list = container.ListBlobs();
            var itemList = new List<CloudStorageItem>();

            var readPolicy = new Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPolicy() {
                Permissions = Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPermissions.Read, // SharedAccessPermissions.Read,
                SharedAccessExpiryTime = DateTime.UtcNow + TimeSpan.FromMinutes(10)
            };

            foreach (var blob in list.OfType<Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob>().OrderByDescending(x => x.Name)) {
                blob.FetchAttributes();
                var item = new CloudStorageItem {
                    Name = blob.Name,
                    Url = new Uri(blob.Uri.AbsoluteUri + blob.GetSharedAccessSignature(readPolicy)).ToString(), //  blob.Uri.AbsolutePath
                    ContentMD5 = blob.Properties.ContentMD5
                };
                itemList.Add(item);
            }

            var model = new GetTimbradoLogViewModel {
                CloudStorageItems = itemList
            };

            return View(model);
        }

        public ActionResult DatabaseSeed() {
            var config = new Migrations.Configuration();
            config.ReSeed(this.DBContext);
            return RedirectToAction("Index");
        }
    }
}