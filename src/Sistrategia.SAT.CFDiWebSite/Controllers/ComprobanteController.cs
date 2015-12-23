using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistrategia.SAT.CFDiWebSite.Models;
using Sistrategia.SAT.CFDiWebSite.CFDI;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Sistrategia.SAT.CFDiWebSite.Controllers
{
    [Authorize]
    public class ComprobanteController : BaseController
    {
        public ActionResult Index() {
            var model = new ComprobanteIndexViewModel {
                Comprobantes = this.DBContext.Comprobantes.ToList()
            };
            return View(model);
        }

        public ActionResult Create() {
            var model = new ComprobanteCreateViewModel();
            model.Conceptos.Add(new ConceptoViewModel());
            model.Conceptos.Add(new ConceptoViewModel());
            model.Conceptos.Add(new ConceptoViewModel());
            model.Conceptos.Add(new ConceptoViewModel());

            var emisores = DBContext.Emisores.ToList();
            var emisoresSelectList = new List<SelectListItem>();
            foreach (var emisor in emisores) {
                emisoresSelectList.Add(new SelectListItem {
                    Value = emisor.EmisorId.ToString(),
                    Text = emisor.Nombre + " - " + emisor.RFC
                });
            }
            model.Emisores = emisoresSelectList;

            var receptores = DBContext.Receptores.ToList();
            var receptoresSelectList = new List<SelectListItem>();
            foreach (var receptor in receptores) {
                receptoresSelectList.Add(new SelectListItem {
                    Value = receptor.ReceptorId.ToString(),
                    Text = receptor.Nombre + " - " + receptor.RFC
                });
            }
            model.Receptores = receptoresSelectList;

            var certificados = DBContext.Certificados.ToList();
            var certificadosSelectList = new List<SelectListItem>();
            foreach (var certificado in certificados) {
                certificadosSelectList.Add(new SelectListItem {
                    Value = certificado.CertificadoId.ToString(),
                    Text = certificado.NumSerie // + " - " + certificado.RFC
                });
            }
            model.Certificados = certificadosSelectList;

            model.FormaDePago = "PAGO EN UNA SOLA EXHIBICION";
            model.MetodoDePago = "NO IDENTIFICADO";
            model.LugarExpedicion = "MATRIZ";
            model.TipoCambio = "1.00";

            //model.Receptores = 
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(ComprobanteCreateViewModel model) {
            

            //var comprobante = DBContext.Comprobantes.Where(e => e.PublicKey == publicKey).SingleOrDefault();

            //if (comprobante == null)
            //    return HttpNotFound();

            //var model = new ComprbanteDetailViewModel(comprobante);

            var comprobante = new Comprobante();

            comprobante.EmisorId = model.EmisorId;
            comprobante.ReceptorId = model.ReceptorId;
            comprobante.Serie = model.Serie;
            comprobante.Folio = model.Folio;
            comprobante.Fecha = DateTime.Now;
            comprobante.FormaDePago = model.FormaDePago;
            comprobante.SubTotal = model.SubTotal;
            comprobante.Total = model.Total;

            //comprobante.NoCertificado;
            //comprobante.Certificado;
            comprobante.TipoDeComprobante = "ingreso";

            comprobante.FormaDePago = model.FormaDePago;
            comprobante.MetodoDePago = model.MetodoDePago;
            comprobante.LugarExpedicion = model.LugarExpedicion;
            comprobante.TipoCambio = model.TipoCambio;

            comprobante.NumCtaPago = model.NumCtaPago;

            comprobante.Conceptos = new List<Concepto>();

            foreach (var modelConcepto in model.Conceptos) {
                if (!string.IsNullOrEmpty(modelConcepto.Descripcion)) {
                    comprobante.Conceptos.Add(new Concepto {
                        Cantidad = modelConcepto.Cantidad,
                        Unidad = modelConcepto.Unidad,
                        NoIdentificacion = modelConcepto.NoIdentificacion,
                        Descripcion = modelConcepto.Descripcion,
                        ValorUnitario = modelConcepto.ValorUnitario,
                        Importe = modelConcepto.Importe,
                        PublicKey = Guid.NewGuid()
                    });
                }
            }

            if (model.IVA > 0) {
                comprobante.Impuestos = new Impuestos();
                comprobante.Impuestos.TotalImpuestosTrasladados = model.IVA;
                comprobante.Impuestos.Traslados = new List<Traslado>();
                comprobante.Impuestos.Traslados.Add(new Traslado {
                    Impuesto = "IVA",
                    Tasa = model.TasaIVA, //  16.00M,
                    Importe = model.IVA
                });
            }

            comprobante.PublicKey = Guid.NewGuid();

            Certificado certificado = DBContext.Certificados.Find(model.CertificadoId);

            if (certificado != null) {
                comprobante.NoCertificado = certificado.NumSerie;
                comprobante.Certificado = certificado.CertificadoBase64;
            }

            comprobante.Fecha = DateTime.Parse("2014-09-03T13:39:03");

            string cadenaOriginal = comprobante.GetCadenaOriginal();
            comprobante.Sello = certificado.GetSello(cadenaOriginal);

            DBContext.Comprobantes.Add(comprobante);
            DBContext.SaveChanges();



            return RedirectToAction("Details", new { id = comprobante.PublicKey });

            //return View(model);
        }

        public ActionResult Details(string id) {
            Guid publicKey;
            if (!Guid.TryParse(id, out publicKey))
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var comprobante = DBContext.Comprobantes.Where(e => e.PublicKey == publicKey).SingleOrDefault();

            if (comprobante == null)
                return HttpNotFound();

            var model = new ComprobanteDetailViewModel(comprobante);
            return View(model);
        }

        public ActionResult ShowXml(string id) {

            Guid publicKey;
            if (!Guid.TryParse(id, out publicKey))
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var comprobante = DBContext.Comprobantes.Where(e => e.PublicKey == publicKey).SingleOrDefault();
            
            if (comprobante == null)
                return HttpNotFound();

            var certificado = DBContext.Certificados.Where(e => e.NumSerie == comprobante.NoCertificado).SingleOrDefault();

            //System.IO.MemoryStream ms = new System.IO.MemoryStream();
            //CFDIXmlTextWriter writer =
            //    new CFDIXmlTextWriter(comprobante, ms, System.Text.Encoding.UTF8);
            //writer.WriteXml();
            //ms.Position = 0;
            //System.IO.StreamReader reader = new System.IO.StreamReader(ms);
            //string xml = reader.ReadToEnd();
            //reader.Close();
            //writer.Close();

            //string xml = comprobante.GetXml();
            string cadenaOriginal = comprobante.GetCadenaOriginal();

            Response.ClearContent();
            Response.ContentType = "application/xml";
            Response.ContentEncoding = System.Text.Encoding.UTF8;

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CFDIXmlTextWriter writer =
                new CFDIXmlTextWriter(comprobante, /*ms*/Response.OutputStream, System.Text.Encoding.UTF8);
            writer.WriteXml();
            ms.Position = 0;
            writer.Close();

            return File(ms, "text/xml");
        }

        public ActionResult ShowCadenaOriginal(string id) {

            Guid publicKey;
            if (!Guid.TryParse(id, out publicKey))
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var comprobante = DBContext.Comprobantes.Where(e => e.PublicKey == publicKey).SingleOrDefault();

            if (comprobante == null)
                return HttpNotFound();

            var certificado = DBContext.Certificados.Where(e => e.NumSerie == comprobante.NoCertificado).SingleOrDefault();

            //System.IO.MemoryStream ms = new System.IO.MemoryStream();
            //CFDIXmlTextWriter writer =
            //    new CFDIXmlTextWriter(comprobante, ms, System.Text.Encoding.UTF8);
            //writer.WriteXml();
            //ms.Position = 0;
            //System.IO.StreamReader reader = new System.IO.StreamReader(ms);
            //string xml = reader.ReadToEnd();
            //reader.Close();
            //writer.Close();

            //string xml = comprobante.GetXml();
            string cadenaOriginal = comprobante.GetCadenaOriginal();

            Response.ClearContent();
            Response.ContentType = "plain/text";
            Response.ContentEncoding = System.Text.Encoding.UTF8;

            //System.IO.MemoryStream ms = new System.IO.MemoryStream();
            //CFDIXmlTextWriter writer =
            //    new CFDIXmlTextWriter(comprobante, /*ms*/Response.OutputStream, System.Text.Encoding.UTF8);
            //writer.WriteXml();
            //ms.Position = 0;
            //writer.Close();

            return Content(cadenaOriginal, "text/plain"); // cadenaOriginal; // File(ms, "text/xml");
        }

        public ActionResult ShowSello(string id) {

            Guid publicKey;
            if (!Guid.TryParse(id, out publicKey))
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var comprobante = DBContext.Comprobantes.Where(e => e.PublicKey == publicKey).SingleOrDefault();

            if (comprobante == null)
                return HttpNotFound();

            var certificado = DBContext.Certificados.Where(e => e.NumSerie == comprobante.NoCertificado).SingleOrDefault();

            //System.IO.MemoryStream ms = new System.IO.MemoryStream();
            //CFDIXmlTextWriter writer =
            //    new CFDIXmlTextWriter(comprobante, ms, System.Text.Encoding.UTF8);
            //writer.WriteXml();
            //ms.Position = 0;
            //System.IO.StreamReader reader = new System.IO.StreamReader(ms);
            //string xml = reader.ReadToEnd();
            //reader.Close();
            //writer.Close();

            //string xml = comprobante.GetXml();
            string cadenaOriginal = comprobante.GetCadenaOriginal();

            

            Response.ClearContent();
            Response.ContentType = "plain/text";
            Response.ContentEncoding = System.Text.Encoding.UTF8;

            //System.IO.MemoryStream ms = new System.IO.MemoryStream();
            //CFDIXmlTextWriter writer =
            //    new CFDIXmlTextWriter(comprobante, /*ms*/Response.OutputStream, System.Text.Encoding.UTF8);
            //writer.WriteXml();
            //ms.Position = 0;
            //writer.Close();

            return Content(certificado.GetSello(cadenaOriginal), "text/plain"); // cadenaOriginal; // File(ms, "text/xml");
        }

        public ActionResult ShowHtml(string id) {
            Guid publicKey;
            if (!Guid.TryParse(id, out publicKey))
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var comprobante = DBContext.Comprobantes.Where(e => e.PublicKey == publicKey).SingleOrDefault();

            if (comprobante == null)
                return HttpNotFound();

            var model = new ComprobanteHtmlViewModel(comprobante);

            if (comprobante.ViewTemplate != null) {
                return View(comprobante.ViewTemplate.CodeName, model);
            }
            else if (comprobante.Emisor.ViewTemplate != null) {
                return View(comprobante.Emisor.ViewTemplate.CodeName, model);
            }

            return View(model);
        }

        public ActionResult GetTimbre(string id) {

            Guid publicKey;
            if (!Guid.TryParse(id, out publicKey))
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var comprobante = DBContext.Comprobantes.Where(e => e.PublicKey == publicKey).SingleOrDefault();

            if (comprobante == null)
                return HttpNotFound();

            var certificado = DBContext.Certificados.Where(e => e.NumSerie == comprobante.NoCertificado).SingleOrDefault();

            var model = new ComprobanteDetailViewModel(comprobante);
            return View(model);
        }

        [HttpPost]
        public ActionResult GetTimbre(string id, FormCollection formCollection) {
        //public ActionResult GetTimbre(string id, ComprobanteDetailViewModel model) {
        //public ActionResult GetTimbre(ComprobanteDetailViewModel model) {
            Guid publicKey;
            if (!Guid.TryParse(id, out publicKey))
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var comprobante = DBContext.Comprobantes.Where(e => e.PublicKey == publicKey).SingleOrDefault();

            if (comprobante == null)
                return HttpNotFound();

            var certificado = DBContext.Certificados.Where(e => e.NumSerie == comprobante.NoCertificado).SingleOrDefault();

            string user = ConfigurationManager.AppSettings["CfdiServiceUser"];
            string password = ConfigurationManager.AppSettings["CfdiServicePassword"];

            var model = new ComprobanteDetailViewModel(comprobante);

            string invoiceFileName = DateTime.Now.ToString("yyyyMMddHmmss_" + comprobante.PublicKey.ToString("N"));
            //comprobante.WriteXml(invoicesPath + invoiceFileName + "_send.xml");



            //// Comprimir y enviar al servicio web
            //string pathFile = invoicesPath + invoiceFileName + "_send.xml";
            //Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile();
            //string saveToFilePath = invoicesPath + invoiceFileName + "_send.zip";
            //zip.AddFile(pathFile, "");
            //zip.Save(saveToFilePath);

            //string filePath = invoicesPath + invoiceFileName + "_send.zip";
            //string responsePath = invoicesPath + invoiceFileName + "_response.zip";

            

            try {

                using (MemoryStream ms = new MemoryStream()) {
                    using (MemoryStream zipMs = new MemoryStream()) {
                        CFDIXmlTextWriter writer =
                            new CFDIXmlTextWriter(comprobante, ms, System.Text.Encoding.UTF8);
                        writer.WriteXml();
                        ms.Position = 0;
                        // writer.Close(); // NO porque cierra a ms

                        using (ZipArchive zip = new ZipArchive(zipMs, ZipArchiveMode.Create, true)) {
                            var entry = zip.CreateEntry(invoiceFileName + "_send.xml");
                            using (Stream s = entry.Open()) {
                                ms.CopyTo(s);
                            }
                            zipMs.Flush();
                        }


                        CloudStorageAccount account = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["AzureDefaultStorageConnectionString"]);
                        CloudBlobClient client = account.CreateCloudBlobClient();

                        // Create the blob storage container 
                        CloudBlobContainer container = client.GetContainerReference(ConfigurationManager.AppSettings["AzureDefaultStorage"]);
                        // container.CreateIfNotExists();

                        // Create the blob in the container 
                        CloudBlockBlob blob = container.GetBlockBlobReference(invoiceFileName + "_send.zip");

                        //zipMs.Flush();

                        //zip.Dispose();
                        zipMs.Position = 0;

                        // Upload the zip and store it in the blob 
                        // using (FileStream fs = zip zipFile.OpenRead())
                        blob.UploadFromStream(zipMs);
                        //zipMs.Close();
                        blob.Properties.ContentType = "application/x-zip-compressed";
                        blob.SetMetadata();
                        blob.SetProperties();
                        // zip.Dispose();
                        //zipMs.Dispose();




                        //byte[] file = null;
                        //int lenght = (int)ms.Length;

                        //file = new byte[lenght];
                        //int count;
                        //int sum = 0;

                        //while ((count = ms.Read(file, sum, lenght - sum)) > 0)
                        //    sum += count;

                        // ms.Close();
                    } //zipMs.Close();
                } // ms.Close();

                //byte[] response = Sistrategia.Server.SAT.SATManager.GetCFDI(user, password, file);

            //    byte[] response = Sistrategia.Server.SAT.SATManager.GetCFDI(user, password, filePath, responsePath);
            //    Ionic.Zip.ZipFile zipR = Ionic.Zip.ZipFile.Read(invoicesPath + invoiceFileName + "_response.zip");
            //    zipR.ExtractAll(invoicesPath, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently);
            //    zipR.Dispose();
            //    //return File(invoicesPath + "SIGN_" + invoiceFileName + "_send.xml", "text/xml");

            //    /* Insert Timbre */
            //    System.Xml.XmlDocument invoice = new System.Xml.XmlDocument();
            //    invoice.Load(invoicesPath + "SIGN_" + invoiceFileName + "_send.xml");
            //    System.Xml.XmlNamespaceManager nsmgr = new System.Xml.XmlNamespaceManager(invoice.NameTable);
            //    nsmgr.AddNamespace("cfdi", "http://www.sat.gob.mx/cfd/3");
            //    nsmgr.AddNamespace("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital");
            //    System.Xml.XmlNode timbre = invoice.SelectSingleNode("//tfd:TimbreFiscalDigital", nsmgr);

            //    Sistrategia.Server.SAT.CFDI.Comprobante comprobante2 = Sistrategia.Server.SAT.SATManager.GetComprobante(Guid.Parse(post["comprobanteId"]));
            //    comprobante2.Complemento = new Sistrategia.Server.SAT.CFDI.ComprobanteComplemento();
            //    comprobante2.Complemento.TimbreFiscalDigitalSpecified = true;
            //    comprobante2.Complemento.TimbreFiscalDigital = new Sistrategia.Server.SAT.CFDI.ComprobanteTimbre();
            //    comprobante2.Complemento.TimbreFiscalDigital.SatTimbreId = Guid.NewGuid();
            //    comprobante2.Complemento.TimbreFiscalDigital.Version = timbre.Attributes.GetNamedItem("version").Value.ToString();
            //    comprobante2.Complemento.TimbreFiscalDigital.UUID = timbre.Attributes.GetNamedItem("UUID").Value.ToString();
            //    comprobante2.Complemento.TimbreFiscalDigital.FechaTimbrado = DateTime.Parse(timbre.Attributes.GetNamedItem("FechaTimbrado").Value);
            //    comprobante2.Complemento.TimbreFiscalDigital.SelloCFD = timbre.Attributes.GetNamedItem("selloCFD").Value.ToString();
            //    comprobante2.Complemento.TimbreFiscalDigital.NoCertificadoSAT = timbre.Attributes.GetNamedItem("noCertificadoSAT").Value.ToString();
            //    comprobante2.Complemento.TimbreFiscalDigital.SelloSAT = timbre.Attributes.GetNamedItem("selloSAT").Value.ToString();

            //    string invoiceXml = string.Empty;
            //    StreamReader streamReader = new StreamReader(invoicesPath + "SIGN_" + invoiceFileName + "_send.xml");
            //    invoiceXml = streamReader.ReadToEnd();
            //    streamReader.Close();

            //    if (Sistrategia.Server.SAT.SATManager.InsertComprobanteTimbre(comprobante2)) {
            //        string QRCODE = "?re=" + comprobante.Emisor.RFC + "&rr=" + comprobante.Receptor.RFC + "&tt=" + comprobante.Total + "&id=" + comprobante2.Complemento.TimbreFiscalDigital.UUID;
            //        TempData["msg2"] = "¡Timbrado exitoso!";
            //    }
            //    /* Insert Timbre */

            //    return RedirectToAction("View", "Invoice", new { id = comprobante.ComprobanteId.ToString() });
            }
            catch (Exception ex) {
                TempData["msg"] = ex.Message.ToString();
                return View(model);
            //    return View();
            }



            
            return View(model);
        }
    }
}