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
        public ActionResult Index()
        {
            this.ViewBag.cfdiService = ConfigurationManager.AppSettings["cfdiService"];
            this.ViewBag.cfdiServiceTimeSpan = SATManager.GetCFDIServiceTimeSpan().Minutes.ToString();

            return View();
        }


        public ActionResult GetTimbradoLog()
        {

            CloudStorageAccount account = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["AzureDefaultStorageConnectionString"]);
            CloudBlobClient client = account.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference(ConfigurationManager.AppSettings["AzureDefaultStorage"]);

            var list = container.ListBlobs();
            var itemList = new List<CloudStorageItem>();

            var readPolicy = new Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPolicy()
            {
                Permissions = Microsoft.WindowsAzure.Storage.Blob.SharedAccessBlobPermissions.Read, // SharedAccessPermissions.Read,
                SharedAccessExpiryTime = DateTime.UtcNow + TimeSpan.FromMinutes(10)
            };

            foreach (var blob in list.OfType<Microsoft.WindowsAzure.Storage.Blob.CloudBlockBlob>().OrderByDescending(x => x.Name))
            {
                blob.FetchAttributes();
                var item = new CloudStorageItem
                {
                    Name = blob.Name,
                    Url = new Uri(blob.Uri.AbsoluteUri + blob.GetSharedAccessSignature(readPolicy)).ToString(), //  blob.Uri.AbsolutePath
                    ContentMD5 = blob.Properties.ContentMD5
                };
                itemList.Add(item);
            }

            var model = new GetTimbradoLogViewModel
            {
                CloudStorageItems = itemList
            };

            return View(model);
        }

        public ActionResult DatabaseSeed()
        {
            var config = new Migrations.Configuration();
            config.ReSeed(this.DBContext);
            return RedirectToAction("Index");
        }

        public ActionResult TestCFDI33()
        {
            var comprobante = new CFDI.Comprobante("3.3");

            comprobante.DecimalFormat = "0.00";
            comprobante.TipoDeComprobante = "I";

            comprobante.Serie = "A";
            comprobante.Folio = "167ABC";
            comprobante.Fecha = DateTime.Parse("2018-01-03T16:57:04");

            CFDI.Certificado certificado = DBContext.Certificados.Find(3);

            if (certificado != null)
            {
                comprobante.CertificadoId = certificado.CertificadoId;
                comprobante.Certificado = certificado;
                comprobante.HasNoCertificado = true;
                comprobante.HasCertificado = true;
            }

            comprobante.FormaPago = "01";
            comprobante.MetodoPago = "PUE";
            comprobante.Moneda = "MXN";
            comprobante.TipoDeComprobante = "I";
            comprobante.CondicionesDePago = "CONDICIONES";
            comprobante.Descuento = 0.0M;
            comprobante.TipoCambio = "1";
            comprobante.LugarExpedicion = "45079";
            comprobante.SubTotal = 100m;
            comprobante.Total = 116m;

            CFDI.Receptor receptor = DBContext.Receptores.Find(54);
            receptor.UsoCFDI = "P01";

            CFDI.ComprobanteReceptor comprobanteReceptor = DBContext.ComprobantesReceptores.Find(51);

            // Crear uno nuevo
            if (comprobanteReceptor == null)
            {
                comprobanteReceptor = new CFDI.ComprobanteReceptor
                {
                    Receptor = receptor
                };
            }

            comprobante.Receptor = comprobanteReceptor;

            CFDI.Emisor emisor = DBContext.Emisores.Find(1);
            CFDI.ComprobanteEmisor comprobanteEmisor = null;
            comprobanteEmisor = DBContext.ComprobantesEmisores.Find(1);

            List <CFDI.ComprobanteEmisorRegimenFiscal> regimenes = new List<CFDI.ComprobanteEmisorRegimenFiscal>();

            regimenes.Add(new CFDI.ComprobanteEmisorRegimenFiscal()
            {
                RegimenFiscal = new CFDI.RegimenFiscal()
                {
                    RegimenFiscalClave = "601",
                    //Regimen = "601",
                }
            });

            if (comprobanteEmisor == null)
            {
                comprobanteEmisor = new CFDI.ComprobanteEmisor
                {
                    Emisor = emisor,
                    DomicilioFiscal = emisor.DomicilioFiscal,
                    RegimenFiscal = regimenes
                };

            }

            comprobante.Emisor = comprobanteEmisor;


            comprobante.Conceptos = new List<CFDI.Concepto>();

            CFDI.Concepto concepto = new CFDI.Concepto
            {
                Cantidad = 10m,
                Unidad = "NA",
                NoIdentificacion = "NA",
                Descripcion = "PRODUCTO",
                ValorUnitario = 10m,
                Importe = 100m,
                PublicKey = Guid.NewGuid(),
                Ordinal = 1,
                ClaveProdServ = "01010101",
                ClaveUnidad = "F52",
            };

            concepto.Impuestos = new CFDI.ConceptoImpuestos();
            concepto.Impuestos.Traslados = new List<CFDI.ConceptoImpuestosTraslado>();

            concepto.Impuestos.Traslados.Add(new CFDI.ConceptoImpuestosTraslado
            {
                Base = 100m,
                TipoFactor = "Tasa",
                TasaOCuota = 0.160000m,
                Importe = 16m,
                Ordinal = 1,
                Impuesto = "002"
            });

            comprobante.Conceptos.Add(concepto);


            comprobante.Impuestos = new CFDI.Impuestos();
            comprobante.Impuestos.Traslados = new List<CFDI.Traslado>();


            comprobante.Impuestos.Traslados.Add(new CFDI.Traslado
            {
                Importe = 16m,
                Impuesto = "002",
                TasaOCuota = 0.16m,
                TipoFactor = "Tasa"
            });

            comprobante.Impuestos.TotalImpuestosTrasladados = 16m;

            string cadenaOriginal = comprobante.GetCadenaOriginal();
            comprobante.Sello = certificado.GetSello(cadenaOriginal);

            DBContext.Comprobantes.Add(comprobante);
            DBContext.SaveChanges();

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CFDI.CFDIXmlTextWriter writer =
                new CFDI.CFDIXmlTextWriter(comprobante, /*ms*/Response.OutputStream, System.Text.Encoding.UTF8);
            writer.WriteXml();
            ms.Position = 0;
            writer.Close();

            return File(ms, "text/xml");
        }
    }
}