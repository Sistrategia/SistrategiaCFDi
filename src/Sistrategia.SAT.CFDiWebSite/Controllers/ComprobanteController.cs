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

        public JsonResult GetIdByEmisores(string value, int pageSize = 10)
        {
            try
            {
                var emisores = DBContext.Emisores.Where(x => x.Status == "A" && ( x.Nombre.Contains(value) || x.RFC.Contains(value)))
                                                 .Take(pageSize).ToList();

                List<dynamic> itemList = new List<dynamic>();
                foreach (var emisor in emisores)
                {
                   var dynamicItems = new
                    {
                        id = emisor.EmisorId.ToString(),
                        text = emisor.Nombre + " - " + emisor.RFC
                    };
                   itemList.Add(dynamicItems);
                }
                return Json(itemList.ToArray(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { resp = false, error = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetIdByReceptores(string value, int pageSize = 10)
        {
            try
            {
                var receptores = DBContext.Receptores.Where(x => x.Status == "A" && (x.Nombre.Contains(value) || x.RFC.Contains(value)))
                                                 .Take(pageSize).ToList();               

                List<dynamic> itemList = new List<dynamic>();
                foreach (var receptor in receptores)
                {
                    var dynamicItems = new
                    {
                        id = receptor.ReceptorId.ToString(),
                        text = receptor.Nombre + " - " + receptor.RFC
                    };
                    itemList.Add(dynamicItems);
                }
                return Json(itemList.ToArray(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { resp = false, error = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetIdByCertificados(int emisorId, int pageSize = 10) {
            try {
                var emisor = DBContext.Emisores.Where(x => x.EmisorId == emisorId).First();
                var certificados = emisor.Certificados.Where(x => x.Estado == "A").Take(pageSize).ToList();

                List<dynamic> itemList = new List<dynamic>();
                foreach (var certificado in certificados) {
                    var dynamicItems = new {
                        id = certificado.CertificadoId.ToString(),
                        text = certificado.NumSerie
                    };
                    itemList.Add(dynamicItems);
                }
                return Json(itemList.ToArray(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) {
                var result = new { resp = false, error = ex.Message };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult LoadComprobantes(int page, int pageSize, string search = null, string sort = null, string sortDir = null) {
            sortDir = string.IsNullOrEmpty(sortDir) ? "asc" : sortDir;
            List<object> itemList = new List<object>();
            try {

                Func<Comprobante, Object> orderByFunc = null;
                switch (sort) {
                    case "ReceptorNombre":
                        orderByFunc = sl => sl.Receptor.Nombre;
                        break;
                    case "Total":
                        orderByFunc = sl => sl.Total;
                        break;
                    default:
                        orderByFunc = sl => sl.Fecha;
                        break;
                }

                List<Comprobante> Comprobantes = new List<Comprobante>();
                if (search != null)
                    Comprobantes = sortDir == "asc" ? DBContext.Comprobantes.Where(x => x.Receptor.Nombre.Contains(search) || x.Total.ToString().Contains(search)).Take(((page - 1) * pageSize) + pageSize).OrderBy(orderByFunc).Skip(((page - 1) * pageSize)).ToList()
                        : DBContext.Comprobantes.Where(x => x.Receptor.Nombre.Contains(search) || x.Total.ToString().Contains(search)).Take(((page - 1) * pageSize) + pageSize).OrderByDescending(orderByFunc).Skip(((page - 1) * pageSize)).ToList();
                else
                    Comprobantes = sortDir == "asc" ? DBContext.Comprobantes.OrderBy(orderByFunc).Take(((page - 1) * pageSize) + pageSize).Skip(((page - 1) * pageSize)).ToList()
                        : DBContext.Comprobantes.OrderByDescending(orderByFunc).Take(((page - 1) * pageSize) + pageSize).Skip(((page - 1) * pageSize)).ToList();             

                Sistrategia.SAT.CFDiWebSite.CloudStorage.CloudStorageMananger cloudStorage = new Sistrategia.SAT.CFDiWebSite.CloudStorage.CloudStorageMananger();

                if (Comprobantes.Count > 0) {
                    int ComprobantesTotalRows = DBContext.Comprobantes.Count();

                    foreach (Comprobante comprobante in Comprobantes) {
                        var dynamicItems = new {
                            error = false,
                            total_rows = ComprobantesTotalRows,
                            returned_rows = Comprobantes.Count,
                            comprobante_id = comprobante.ComprobanteId,
                            public_key = comprobante.PublicKey,
                            receptor_initial_letter = comprobante.Receptor.Nombre.Substring(0, 1),
                            serie = comprobante.Serie,
                            folio = comprobante.Folio,
                            receptor = comprobante.Receptor.Nombre,
                            fecha = comprobante.Fecha.ToLongDateString(),
                            total = comprobante.Total.ToString("C"),
                            status = comprobante.Status
                        };
                        itemList.Add(dynamicItems);
                    }
                }
            }
            catch (Exception ex) {
                var errorMessage = new {
                    error = true,
                    errorMsg = ex.ToString()
                };
                itemList.Add(errorMessage);
            }

            return Json(itemList);
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
        public JsonResult Create(ComprobanteCreateViewModel model) {
            try
            {
                if (String.IsNullOrEmpty(model.LugarExpedicion))
                    throw new ApplicationException("¡Ingrese el lugar de expedición!");
                else if (model.EmisorId <= 0)
                    throw new ApplicationException("¡Ingrese el emisor!");
                else if (model.ReceptorId <= 0)
                    throw new ApplicationException("¡Ingrese el receptor!");
                else if (model.CertificadoId <= 0)
                    throw new ApplicationException("¡Ingrese el certificado!");
                else if (String.IsNullOrEmpty(model.FormaDePago))
                    throw new ApplicationException("¡Ingrese la forma de pago!");
                else if (String.IsNullOrEmpty(model.MetodoDePago))
                    throw new ApplicationException("¡Ingrese el método de pago!");
                else if ((model.MetodoDePago != "EFECTIVO" && model.MetodoDePago != "NO IDENTIFICADO") && (model.NumCtaPago.Count() > 6 || model.NumCtaPago.Count() < 4))                     throw new ApplicationException("¡El valor de NumCtaPago debe contener entre 4 hasta 6 caracteres!");
                else if ((model.Conceptos != null || model.Conceptos.Count > 0) 
                    && model.Conceptos.All(x => x.Cantidad < 0m || x.Unidad == null || x.Descripcion == null || x.ValorUnitario < 0m))
                    throw new ApplicationException("¡Ingrese al menos un concepto!");
                else if (model.SubTotal < 0m)
                    throw new ApplicationException("¡SubTotal no válido!");
                else if (model.TotalImpuestosTrasladados < 0m)
                    throw new ApplicationException("¡Total Impuestos Trasladados no válido!");
                else if (model.TotalImpuestosRetenidos < 0m)
                    throw new ApplicationException("¡Total Impuestos Retenidos no válido!");
                else if (model.Total < 0m)
                    throw new ApplicationException("¡Total no válido!");
                else {

                    var comprobante = new Comprobante();

                    comprobante.EmisorId = model.EmisorId;
                    comprobante.Emisor = DBContext.Emisores.Find(model.EmisorId); // .Where(e => e.PublicKey == publicKey).SingleOrDefault();
                    comprobante.ReceptorId = model.ReceptorId;
                    comprobante.Receptor = DBContext.Receptores.Find(model.ReceptorId); // .Where(e => e.PublicKey == publicKey).SingleOrDefault();
                    comprobante.Serie = model.Serie;
                    comprobante.Folio = model.Folio;
                    comprobante.Fecha = DateTime.Now + SATManager.GetCFDIServiceTimeSpan();
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

                    comprobante.Impuestos = new Impuestos();
                    comprobante.Impuestos.Traslados = new List<Traslado>();

                    foreach (var modelTraslado in model.Traslados) {
                        if (modelTraslado.Tasa > 0 && modelTraslado.Importe > 0) {
                            comprobante.Impuestos.Traslados.Add(new Traslado {
                                Importe = modelTraslado.Importe,
                                Impuesto = modelTraslado.Impuesto,
                                Tasa = modelTraslado.Tasa,
                            });
                        }
                    }

                    comprobante.Impuestos.Retenciones = new List<Retencion>();
                    foreach (var modelRetencion in model.Retenciones) {
                        if (modelRetencion.Importe > 0) {
                            comprobante.Impuestos.Retenciones.Add(new Retencion {
                                Importe = modelRetencion.Importe,
                                Impuesto = modelRetencion.Impuesto,
                            });
                        }
                    }

                    if (model.TotalImpuestosRetenidos > 0)
                        comprobante.Impuestos.TotalImpuestosRetenidos = model.TotalImpuestosRetenidos;

                    if (model.TotalImpuestosTrasladados > 0)
                        comprobante.Impuestos.TotalImpuestosTrasladados = model.TotalImpuestosTrasladados;

                    comprobante.PublicKey = Guid.NewGuid();

                    Certificado certificado = DBContext.Certificados.Find(model.CertificadoId);

                    if (certificado != null) {
                        // comprobante.NoCertificado = certificado.NumSerie;
                        // comprobante.Certificado = certificado.CertificadoBase64;
                        comprobante.CertificadoId = certificado.CertificadoId;
                        comprobante.Certificado = certificado;
                        comprobante.HasNoCertificado = true;
                        comprobante.HasCertificado = true;
                    }

                    string cadenaOriginal = comprobante.GetCadenaOriginal();
                    comprobante.Sello = certificado.GetSello(cadenaOriginal);

                    DBContext.Comprobantes.Add(comprobante);
                    DBContext.SaveChanges();

                    TempData["success"] = "Se ha creado el comprobante correctamente";
                    var data = new {
                        error = false,
                        errorMsg = "",
                        comprobanteId = comprobante.PublicKey
                    };
                    return Json(data);
                }
            }
            catch (Exception ex)
            {
                var data = new
                {
                    error = true,
                    errorMsg = ex.Message.ToString()
                };
                return Json(data);
            }     
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

        public ActionResult ShowCadenaOriginal64(string id) {

            Guid publicKey;
            if (!Guid.TryParse(id, out publicKey))
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var comprobante = DBContext.Comprobantes.Where(e => e.PublicKey == publicKey).SingleOrDefault();

            if (comprobante == null)
                return HttpNotFound();

            string cadenaOriginal = comprobante.GetCadenaOriginal();
            Response.ClearContent();
            Response.ContentType = "plain/text";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(cadenaOriginal);
            return Content(System.Convert.ToBase64String(plainTextBytes), "text/plain"); // cadenaOriginal; // File(ms, "text/xml");
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

        public ActionResult ShowSello64(string id) {

            Guid publicKey;
            if (!Guid.TryParse(id, out publicKey))
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var comprobante = DBContext.Comprobantes.Where(e => e.PublicKey == publicKey).SingleOrDefault();

            if (comprobante == null)
                return HttpNotFound();

            var certificado = DBContext.Certificados.Where(e => e.NumSerie == comprobante.NoCertificado).SingleOrDefault();
            string cadenaOriginal = comprobante.GetCadenaOriginal();
            Response.ClearContent();
            Response.ContentType = "plain/text";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(certificado.GetSello(cadenaOriginal));
            return Content(System.Convert.ToBase64String(plainTextBytes), "text/plain"); // cadenaOriginal; // File(ms, "text/xml");
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

            

            //manager.GetCFDI(user, password, comprobante, certificado);
            

            //// Comprimir y enviar al servicio web
            //string pathFile = invoicesPath + invoiceFileName + "_send.xml";
            //Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile();
            //string saveToFilePath = invoicesPath + invoiceFileName + "_send.zip";
            //zip.AddFile(pathFile, "");
            //zip.Save(saveToFilePath);

            //string filePath = invoicesPath + invoiceFileName + "_send.zip";
            //string responsePath = invoicesPath + invoiceFileName + "_response.zip";

            

            try {

                SATManager manager = new SATManager();
                bool response = manager.GetCFDI(user, password, comprobante);
                if (response)
                    DBContext.SaveChanges();

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

        public ActionResult GetPDF(string id) {
            Guid publicKey;
            if (!Guid.TryParse(id, out publicKey))
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var comprobante = DBContext.Comprobantes.Where(e => e.PublicKey == publicKey).SingleOrDefault();

            if (comprobante == null)
                return HttpNotFound();

            string PdfFileName = "";

            if (comprobante.Serie == null) {
                PdfFileName = "FACTURA_" + comprobante.Folio.ToString();
            }
            else {
                PdfFileName = "FACTURA_" + comprobante.Folio.ToString() + comprobante.Serie.ToString();
            }
            PdfFileName = PdfFileName + ".pdf";

            try {
                Response.ClearContent();
                Response.ContentType = "application/pdf";
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                InvoicePdfModel pdfGenerator = new InvoicePdfModel();
                return File(pdfGenerator.CreatePDF(comprobante), "application/pdf", PdfFileName);
            }
            catch (Exception ex) {
                TempData["msg2"] = ex.Message.ToString();
                return RedirectToAction("Index");
            }
        }
    }
}