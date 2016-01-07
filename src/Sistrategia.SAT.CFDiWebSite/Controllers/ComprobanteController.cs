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
using Sistrategia.SAT.CFDiWebSite.CloudStorage;

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

        public JsonResult GetIdByEmisores(string value, int pageSize = 10) {
            try {
                var emisores = DBContext.Emisores.Where(x => x.Status == "A" && (x.Nombre.Contains(value) || x.RFC.Contains(value)))
                                                 .Take(pageSize).ToList();

                List<dynamic> itemList = new List<dynamic>();
                foreach (var emisor in emisores) {
                    var dynamicItems = new {
                        id = emisor.EmisorId.ToString(),
                        text = emisor.Nombre + " - " + emisor.RFC
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

        public JsonResult GetIdByReceptores(string value, int pageSize = 10) {
            try {
                var receptores = DBContext.Receptores.Where(x => x.Status == "A" && (x.Nombre.Contains(value) || x.RFC.Contains(value)))
                                                 .Take(pageSize).ToList();

                List<dynamic> itemList = new List<dynamic>();
                foreach (var receptor in receptores) {
                    var dynamicItems = new {
                        id = receptor.ReceptorId.ToString(),
                        text = receptor.Nombre + " - " + receptor.RFC
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
            sortDir = string.IsNullOrEmpty(sortDir) ? "ASC" : sortDir;
            List<object> itemList = new List<object>();
            try {

                Func<Comprobante, Object> orderByFunc = null;
                switch (sort) {
                    case "ReceptorNombre":
                        orderByFunc = sl => sl.Receptor.Nombre;
                        break;
                    case "Fecha":
                        orderByFunc = sl => sl.Fecha;
                        break;
                    case "Total":
                        orderByFunc = sl => sl.Total;
                        break;
                    case "Status":
                        orderByFunc = sl => sl.Status;
                        break;
                    default:
                        orderByFunc = sl => sl.Fecha;
                        break;
                }

                List<Comprobante> Comprobantes = new List<Comprobante>();
                if (search != null)
                    Comprobantes = sortDir == "ASC" ? DBContext.Comprobantes.Where(x => x.Receptor.Nombre.Contains(search) 
                        || x.Total.ToString().Contains(search) 
                        || x.Status.Contains(search)
                        || (x.Serie + x.Folio).Contains(search)
                        ).OrderBy(orderByFunc)
                        .Take(((page - 1) * pageSize) + pageSize)                       
                        .Skip(((page - 1) * pageSize)).ToList()
                        : 
                        DBContext.Comprobantes.Where(x => x.Receptor.Nombre.Contains(search)
                        || x.Total.ToString().Contains(search)
                        || x.Status.Contains(search)
                        || (x.Serie + x.Folio).Contains(search)
                        ).OrderByDescending(orderByFunc)
                        .Take(((page - 1) * pageSize) + pageSize)                        
                        .Skip(((page - 1) * pageSize)).ToList();
                else
                    Comprobantes = sortDir == "ASC" ? DBContext.Comprobantes.OrderBy(orderByFunc).Take(((page - 1) * pageSize) + pageSize).Skip(((page - 1) * pageSize)).ToList()
                        : DBContext.Comprobantes.OrderByDescending(orderByFunc).Take(((page - 1) * pageSize) + pageSize).Skip(((page - 1) * pageSize)).ToList();
               
                if (Comprobantes.Count > 0) {
                    int ComprobantesTotalRows = DBContext.Comprobantes.Where(x => x.Receptor.Nombre.Contains(search) 
                                                                            || x.Total.ToString().Contains(search) 
                                                                            || x.Status.Contains(search)
                                                                            || (x.Serie + x.Folio).Contains(search)
                                                                            ).Count();

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
            try {
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
                else if ((model.MetodoDePago != "EFECTIVO" && model.MetodoDePago != "NO IDENTIFICADO") && (model.NumCtaPago.Count() > 6 || model.NumCtaPago.Count() < 4))
                    throw new ApplicationException("¡El valor de NumCtaPago debe contener entre 4 hasta 6 caracteres!");
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
            catch (Exception ex) {
                var data = new {
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

        public ActionResult Upload() {
            var model = new ComprobanteUploadViewModel();
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(ComprobanteUploadViewModel model) {
            string comprobanteId = "";
            if (ModelState.IsValid) {



                if (model.ComprobanteArchivo == null || model.ComprobanteArchivo.ContentLength == 0) {
                    return View();
                }
                try {

                    Comprobante comprobante = new Comprobante();
                    Certificado certificado = new Certificado();

                    if (model.ComprobanteArchivo != null) {
                        // MemoryStream target = new MemoryStream();

                        System.Xml.XmlTextReader xmlReader = new System.Xml.XmlTextReader(model.ComprobanteArchivo.InputStream);

                        while (xmlReader.Read()) {
                            if (xmlReader.NodeType == System.Xml.XmlNodeType.Element) {

                                if ("xml".Equals(xmlReader.Name)) {

                                }
                                else if ("cfdi:Comprobante".Equals(xmlReader.Name)) {
                                    while (xmlReader.MoveToNextAttribute()) {
                                        switch (xmlReader.Name) {
                                            case "version":
                                                comprobante.Version = xmlReader.Value;
                                                break;
                                            case "serie":
                                                comprobante.Serie = xmlReader.Value;
                                                break;
                                            case "folio":
                                                comprobante.Folio = xmlReader.Value;
                                                break;
                                            case "fecha":
                                                comprobante.Fecha = DateTime.Parse(xmlReader.Value);
                                                break;
                                            case "sello":
                                                comprobante.Sello = xmlReader.Value;
                                                break;
                                            case "noAprobacion":
                                                comprobante.NoAprobacion = xmlReader.Value;
                                                break;
                                            case "anoAprobacion":
                                                comprobante.AnoAprobacion = xmlReader.Value;
                                                break;
                                            case "formaDePago":
                                                comprobante.FormaDePago = xmlReader.Value;
                                                break;
                                            case "noCertificado":
                                                certificado.NumSerie = xmlReader.Value;
                                                //comprobante.LugarExpedicion = xmlReader.Value;
                                                comprobante.HasNoCertificado = true;
                                                break;
                                            case "certificado":
                                                //comprobante.LugarExpedicion = xmlReader.Value;
                                                certificado.CertificadoBase64 = xmlReader.Value;
                                                comprobante.HasCertificado = true;
                                                break;
                                            case "condicionesDePago":
                                                comprobante.CondicionesDePago = xmlReader.Value;
                                                break;
                                            case "subTotal":
                                                comprobante.SubTotal = decimal.Parse(xmlReader.Value);
                                                break;
                                            case "descuento":
                                                comprobante.Descuento = decimal.Parse(xmlReader.Value);
                                                break;
                                            case "motivoDescuento":
                                                comprobante.MotivoDescuento = xmlReader.Value;
                                                break;
                                            case "TipoCambio":
                                                comprobante.TipoCambio = xmlReader.Value;
                                                break;
                                            case "Moneda":
                                                comprobante.Moneda = xmlReader.Value;
                                                break;
                                            case "total":
                                                comprobante.Total = decimal.Parse(xmlReader.Value);
                                                break;
                                            case "tipoDeComprobante":
                                                comprobante.TipoDeComprobante = xmlReader.Value;
                                                break;
                                            case "metodoDePago":
                                                comprobante.MetodoDePago = xmlReader.Value;
                                                break;
                                            case "LugarExpedicion":
                                                comprobante.LugarExpedicion = xmlReader.Value;
                                                break;
                                            case "NumCtaPago":
                                                comprobante.NumCtaPago = xmlReader.Value;
                                                break;
                                            case "SerieFolioFiscalOrig":
                                                comprobante.SerieFolioFiscalOrig = xmlReader.Value;
                                                break;
                                            case "FechaFolioFiscalOrig":
                                                comprobante.FechaFolioFiscalOrig = DateTime.Parse(xmlReader.Value);
                                                break;
                                            case "MontoFolioFiscalOrig":
                                                comprobante.MontoFolioFiscalOrig = decimal.Parse(xmlReader.Value);
                                                break;

                                            case "xmlns:cfdi":
                                            case "xmlns:xsi":
                                            case "xsi:schemaLocation":
                                                break;
                                            default:
                                                throw new Exception(xmlReader.Name + "is not a valid attribute for cfdi:Comprobante.");
                                        }
                                    }


                                }

                                else if ("cfdi:Emisor".Equals(xmlReader.Name)) {
                                    comprobante.Emisor = new Emisor();
                                    while (xmlReader.MoveToNextAttribute()) {
                                        switch (xmlReader.Name) {
                                            case "rfc":
                                                comprobante.Emisor.RFC = xmlReader.Value;
                                                break;
                                            case "nombre":
                                                comprobante.Emisor.Nombre = xmlReader.Value;
                                                break;
                                            default:
                                                throw new Exception(xmlReader.Name + "is not a valid attribute for cfdi:Emisor.");
                                        }
                                    }
                                }

                                else if ("cfdi:DomicilioFiscal".Equals(xmlReader.Name)) {
                                    comprobante.Emisor.DomicilioFiscal = new UbicacionFiscal();
                                    while (xmlReader.MoveToNextAttribute()) {
                                        switch (xmlReader.Name) {
                                            case "calle":
                                                comprobante.Emisor.DomicilioFiscal.Calle = xmlReader.Value;
                                                break;
                                            case "noExterior":
                                                comprobante.Emisor.DomicilioFiscal.NoExterior = xmlReader.Value;
                                                break;
                                            case "noInterior":
                                                comprobante.Emisor.DomicilioFiscal.NoInterior = xmlReader.Value;
                                                break;
                                            case "colonia":
                                                comprobante.Emisor.DomicilioFiscal.Colonia = xmlReader.Value;
                                                break;
                                            case "localidad":
                                                comprobante.Emisor.DomicilioFiscal.Localidad = xmlReader.Value;
                                                break;
                                            case "referencia":
                                                comprobante.Emisor.DomicilioFiscal.Referencia = xmlReader.Value;
                                                break;
                                            case "municipio":
                                                comprobante.Emisor.DomicilioFiscal.Municipio = xmlReader.Value;
                                                break;
                                            case "estado":
                                                comprobante.Emisor.DomicilioFiscal.Estado = xmlReader.Value;
                                                break;
                                            case "pais":
                                                comprobante.Emisor.DomicilioFiscal.Pais = xmlReader.Value;
                                                break;
                                            case "codigoPostal":
                                                comprobante.Emisor.DomicilioFiscal.CodigoPostal = xmlReader.Value;
                                                break;
                                            default:
                                                throw new Exception(xmlReader.Name + "is not a valid attribute for cfdi:DomicilioFiscal.");
                                        }
                                    }
                                }

                                else if ("cfdi:RegimenFiscal".Equals(xmlReader.Name)) {
                                    if (comprobante.Emisor.RegimenFiscal == null)
                                        comprobante.Emisor.RegimenFiscal = new List<RegimenFiscal>();
                                    while (xmlReader.MoveToNextAttribute()) {
                                        switch (xmlReader.Name) {
                                            case "Regimen":
                                                RegimenFiscal regimen = new RegimenFiscal();
                                                regimen.Regimen = xmlReader.Value;
                                                break;
                                            default:
                                                throw new Exception(xmlReader.Name + "is not a valid attribute for cfdi:RegimenFiscal.");
                                        }
                                    }
                                }

                                else if ("cfdi:Receptor".Equals(xmlReader.Name)) {
                                    comprobante.Receptor = new Receptor();
                                    while (xmlReader.MoveToNextAttribute()) {
                                        switch (xmlReader.Name) {
                                            case "rfc":
                                                comprobante.Receptor.RFC = xmlReader.Value;
                                                break;
                                            case "nombre":
                                                comprobante.Receptor.Nombre = xmlReader.Value;
                                                break;
                                            default:
                                                throw new Exception(xmlReader.Name + "is not a valid attribute for cfdi:Receptor.");
                                        }
                                    }
                                }

                                else if ("cfdi:Domicilio".Equals(xmlReader.Name)) {
                                    comprobante.Receptor.Domicilio = new Ubicacion();
                                    while (xmlReader.MoveToNextAttribute()) {
                                        switch (xmlReader.Name) {
                                            case "calle":
                                                comprobante.Receptor.Domicilio.Calle = xmlReader.Value;
                                                break;
                                            case "noExterior":
                                                comprobante.Receptor.Domicilio.NoExterior = xmlReader.Value;
                                                break;
                                            case "noInterior":
                                                comprobante.Receptor.Domicilio.NoInterior = xmlReader.Value;
                                                break;
                                            case "colonia":
                                                comprobante.Receptor.Domicilio.Colonia = xmlReader.Value;
                                                break;
                                            case "localidad":
                                                comprobante.Receptor.Domicilio.Localidad = xmlReader.Value;
                                                break;
                                            case "referencia":
                                                comprobante.Receptor.Domicilio.Referencia = xmlReader.Value;
                                                break;
                                            case "municipio":
                                                comprobante.Receptor.Domicilio.Municipio = xmlReader.Value;
                                                break;
                                            case "estado":
                                                comprobante.Receptor.Domicilio.Estado = xmlReader.Value;
                                                break;
                                            case "pais":
                                                comprobante.Receptor.Domicilio.Pais = xmlReader.Value;
                                                break;
                                            case "codigoPostal":
                                                comprobante.Receptor.Domicilio.CodigoPostal = xmlReader.Value;
                                                break;
                                            default:
                                                throw new Exception(xmlReader.Name + "is not a valid attribute for cfdi:Domicilio.");
                                        }
                                    }
                                }

                                else if ("cfdi:Conceptos".Equals(xmlReader.Name)) {
                                    comprobante.Conceptos = new List<Concepto>();
                                }

                                else if ("cfdi:Concepto".Equals(xmlReader.Name)) {
                                    Concepto concepto = new Concepto();
                                    concepto.PublicKey = Guid.NewGuid();
                                    while (xmlReader.MoveToNextAttribute()) {
                                        switch (xmlReader.Name) {
                                            case "cantidad":
                                                concepto.Cantidad = decimal.Parse(xmlReader.Value);
                                                break;
                                            case "unidad":
                                                concepto.Unidad = xmlReader.Value;
                                                break;
                                            case "noIdentificacion":
                                                concepto.NoIdentificacion = xmlReader.Value;
                                                break;
                                            case "descripcion":
                                                concepto.Descripcion = xmlReader.Value;
                                                break;
                                            case "valorUnitario":
                                                concepto.ValorUnitario = decimal.Parse(xmlReader.Value);
                                                break;
                                            case "importe":
                                                concepto.Importe = decimal.Parse(xmlReader.Value);
                                                break;
                                            default:
                                                throw new Exception(xmlReader.Name + "is not a valid attribute for cfdi:Domicilio.");
                                        }
                                    }
                                    concepto.Ordinal = comprobante.Conceptos.Count + 1;
                                    comprobante.Conceptos.Add(concepto);
                                }

                                else if ("cfdi:Impuestos".Equals(xmlReader.Name)) {
                                    comprobante.Impuestos = new Impuestos();
                                    while (xmlReader.MoveToNextAttribute()) {
                                        switch (xmlReader.Name) {
                                            case "totalImpuestosRetenidos":
                                                comprobante.Impuestos.TotalImpuestosRetenidos = decimal.Parse(xmlReader.Value);
                                                break;
                                            case "totalImpuestosTrasladados":
                                                comprobante.Impuestos.TotalImpuestosTrasladados = decimal.Parse(xmlReader.Value);
                                                break;
                                            default:
                                                throw new Exception(xmlReader.Name + "is not a valid attribute for cfdi:Impuestos.");
                                        }
                                    }
                                }

                                else if ("cfdi:Traslados".Equals(xmlReader.Name)) {
                                    comprobante.Impuestos.Traslados = new List<Traslado>();
                                }

                                else if ("cfdi:Traslado".Equals(xmlReader.Name)) {
                                    Traslado traslado = new Traslado();
                                    while (xmlReader.MoveToNextAttribute()) {
                                        switch (xmlReader.Name) {
                                            case "impuesto":
                                                traslado.Impuesto = xmlReader.Value;
                                                break;
                                            case "tasa":
                                                traslado.Tasa = decimal.Parse(xmlReader.Value);
                                                break;
                                            case "importe":
                                                traslado.Importe = decimal.Parse(xmlReader.Value);
                                                break;
                                            default:
                                                throw new Exception(xmlReader.Name + "is not a valid attribute for cfdi:Impuestos.");
                                        }
                                    }
                                    comprobante.Impuestos.Traslados.Add(traslado);
                                }

                                else if ("cfdi:Retenciones".Equals(xmlReader.Name)) {
                                    comprobante.Impuestos.Retenciones = new List<Retencion>();
                                }

                                else if ("cfdi:Retencion".Equals(xmlReader.Name)) {
                                    Retencion retencion = new Retencion();
                                    while (xmlReader.MoveToNextAttribute()) {
                                        switch (xmlReader.Name) {
                                            case "impuesto":
                                                retencion.Impuesto = xmlReader.Value;
                                                break;
                                            case "importe":
                                                retencion.Importe = decimal.Parse(xmlReader.Value);
                                                break;
                                            default:
                                                throw new Exception(xmlReader.Name + "is not a valid attribute for cfdi:Retencion.");
                                        }
                                    }
                                    comprobante.Impuestos.Retenciones.Add(retencion);
                                }

                                else if ("cfdi:Complemento".Equals(xmlReader.Name)) {
                                    comprobante.Complementos = new List<Complemento>();
                                }

                                else if ("tfd:TimbreFiscalDigital".Equals(xmlReader.Name)) {
                                    TimbreFiscalDigital timbre = new TimbreFiscalDigital();
                                    while (xmlReader.MoveToNextAttribute()) {
                                        switch (xmlReader.Name) {
                                            case "version":
                                                timbre.Version = xmlReader.Value;
                                                break;
                                            case "UUID":
                                                timbre.UUID = xmlReader.Value;
                                                break;
                                            case "FechaTimbrado":
                                                timbre.FechaTimbrado = DateTime.Parse(xmlReader.Value);
                                                break;
                                            case "selloCFD":
                                                timbre.SelloCFD = xmlReader.Value;
                                                break;
                                            case "noCertificadoSAT":
                                                timbre.NoCertificadoSAT = xmlReader.Value;
                                                break;
                                            case "selloSAT":
                                                timbre.SelloSAT = xmlReader.Value;
                                                break;
                                            case "xmlns:tfd":
                                            case "xsi:schemaLocation":
                                                break;
                                            default:
                                                throw new Exception(xmlReader.Name + " is not a valid attribute for cfdi:TimbreFiscalDigital.");
                                        }
                                    }
                                    comprobante.Complementos.Add(timbre);
                                }

                                else {
                                    xmlReader.NodeType.ToString();
                                    xmlReader.Name.ToString();
                                }
                                //xmlReader.NodeType.ToString();
                            }
                        }

                        //xmlReader.Dispose();
                        //xmlReader.Close();

                        model.ComprobanteArchivo.InputStream.Position = 0;
                        //model.ComprobanteArchivo.InputStream.Position = 0;

                        //model.ComprobanteArchivo.InputStream.CopyTo(target);
                        //Byte[] data = target.ToArray();

                        if (certificado != null) {
                            if (!string.IsNullOrEmpty(certificado.NumSerie)) {
                                certificado = DBContext.Certificados.Where(c => c.NumSerie == certificado.NumSerie).SingleOrDefault();
                                comprobante.Certificado = certificado;
                            }
                        }

                        if (comprobante.Emisor != null) {
                            //Emisor emisor = DBContext.Emisores.Where(e => 
                            //    (e.RFC == comprobante.Emisor.RFC)
                            //    && (e.Nombre == comprobante.Emisor.Nombre)                                
                            //    && (e.Status == "A")
                            //    ).SingleOrDefault();

                            List<Emisor> emisores = DBContext.Emisores.Where(e =>
                                (e.RFC == comprobante.Emisor.RFC)
                                && (e.Nombre == comprobante.Emisor.Nombre)
                                ).ToList();

                            if (emisores != null && emisores.Count > 0) {
                                foreach (Emisor emisor in emisores) {

                                    if ((emisor.DomicilioFiscal != null && comprobante.Emisor.DomicilioFiscal != null)
                                        && (emisor.DomicilioFiscal.Calle == comprobante.Emisor.DomicilioFiscal.Calle)
                                        && (emisor.DomicilioFiscal.NoExterior == comprobante.Emisor.DomicilioFiscal.NoExterior)
                                        && (emisor.DomicilioFiscal.NoInterior == comprobante.Emisor.DomicilioFiscal.NoInterior)
                                        && (emisor.DomicilioFiscal.Colonia == comprobante.Emisor.DomicilioFiscal.Colonia)
                                        && (emisor.DomicilioFiscal.Referencia == comprobante.Emisor.DomicilioFiscal.Referencia)
                                        && (emisor.DomicilioFiscal.Localidad == comprobante.Emisor.DomicilioFiscal.Localidad)
                                        && (emisor.DomicilioFiscal.Municipio == comprobante.Emisor.DomicilioFiscal.Municipio)
                                        && (emisor.DomicilioFiscal.Estado == comprobante.Emisor.DomicilioFiscal.Estado)
                                        && (emisor.DomicilioFiscal.CodigoPostal == comprobante.Emisor.DomicilioFiscal.CodigoPostal)
                                        && (emisor.DomicilioFiscal.Pais == comprobante.Emisor.DomicilioFiscal.Pais)
                                        ) {

                                        //if (receptor != null) {
                                        comprobante.Emisor = emisor;
                                        comprobante.EmisorId = emisor.EmisorId;
                                    }
                                }
                                if (comprobante.EmisorId == null) {
                                    // The address has changed, create a new one and inactive the oldone

                                    foreach (Emisor emisor in emisores) {
                                        emisor.Status = "I";
                                    }

                                    comprobante.Emisor.Status = "A";

                                }

                            }
                            else {
                                comprobante.Emisor.Status = "A";
                            }
                        }

                        if (comprobante.Receptor != null) {
                            //Receptor receptor = DBContext.Receptores.Where(r =>
                            //    (r.RFC == comprobante.Receptor.RFC)
                            //    && (r.Nombre == comprobante.Receptor.Nombre)
                            //    && (r.Status == "A")
                            //    ).SingleOrDefault();

                            List<Receptor> receptores = DBContext.Receptores.Where(r =>
                                (r.RFC == comprobante.Receptor.RFC)
                                && (r.Nombre == comprobante.Receptor.Nombre)
                                ).ToList();

                            if (receptores != null && receptores.Count > 0) {
                                foreach (Receptor receptor in receptores) {

                                    if ((receptor.Domicilio != null && comprobante.Receptor.Domicilio != null)
                                        && (receptor.Domicilio.Calle == comprobante.Receptor.Domicilio.Calle)
                                        && (receptor.Domicilio.NoExterior == comprobante.Receptor.Domicilio.NoExterior)
                                        && (receptor.Domicilio.NoInterior == comprobante.Receptor.Domicilio.NoInterior)
                                        && (receptor.Domicilio.Colonia == comprobante.Receptor.Domicilio.Colonia)
                                        && (receptor.Domicilio.Referencia == comprobante.Receptor.Domicilio.Referencia)
                                        && (receptor.Domicilio.Localidad == comprobante.Receptor.Domicilio.Localidad)
                                        && (receptor.Domicilio.Municipio == comprobante.Receptor.Domicilio.Municipio)
                                        && (receptor.Domicilio.Estado == comprobante.Receptor.Domicilio.Estado)
                                        && (receptor.Domicilio.CodigoPostal == comprobante.Receptor.Domicilio.CodigoPostal)
                                        && (receptor.Domicilio.Pais == comprobante.Receptor.Domicilio.Pais)
                                        ) {

                                        //if (receptor != null) {
                                        comprobante.Receptor = receptor;
                                        comprobante.ReceptorId = receptor.ReceptorId;
                                    }
                                }
                                if (comprobante.ReceptorId == null) {
                                    // The address has changed, create a new one and inactive the oldone

                                    foreach (Receptor receptor in receptores) {
                                        receptor.Status = "I";
                                    }

                                    comprobante.Receptor.Status = "A";

                                }
                            }
                            else {
                                comprobante.Receptor.Status = "A";
                            }

                            //if (receptor != null) {
                            //    comprobante.Receptor = receptor;
                            //    comprobante.ReceptorId = receptor.ReceptorId;
                            //}
                            //else {

                            //}
                        }

                        comprobante.GeneratedCadenaOriginal = comprobante.GetCadenaOriginal();

                        if (model.ComprobantePDFArchivo != null && model.ComprobantePDFArchivo.ContentLength > 0) {
                            comprobante.GeneratedXmlUrl = string.Format(@"https://sistrategiacfdi1.blob.core.windows.net/{0}/{1}.xml",
                                comprobante.Emisor.PublicKey.ToString("N"),
                                comprobante.PublicKey.ToString("N"));
                            comprobante.GeneratedPDFUrl = string.Format(@"https://sistrategiacfdi1.blob.core.windows.net/{0}/{1}.pdf",
                                comprobante.Emisor.PublicKey.ToString("N"),
                                comprobante.PublicKey.ToString("N"));
                            //comprobante.GeneratedPDFUrl
                            //comprobante.ExtendedIntValue1 = model.NoOrden;
                            //comprobante.ExtendedIntValue2 = model.NoCliente;
                        }

                        comprobante.ExtendedIntValue1 = DBContext.Comprobantes.Max(c => c.ExtendedIntValue1) + 1; // DBContext.Comprobantes.Count() + 1;
                        if (comprobante.ReceptorId != null)
                            comprobante.ExtendedIntValue2 = comprobante.ReceptorId;
                        else
                            comprobante.ExtendedIntValue2 = DBContext.Receptores.Count() + 1;

                        comprobante.ViewTemplate = DBContext.ViewTemplates.Find(2);
                        comprobante.ViewTemplateId = comprobante.ViewTemplate.ViewTemplateId;

                        comprobante.Status = "A";

                        comprobanteId = comprobante.PublicKey.ToString("N");
                        DBContext.Comprobantes.Add(comprobante);
                        DBContext.SaveChanges();

                        if (model.ComprobantePDFArchivo != null && model.ComprobantePDFArchivo.ContentLength > 0) {
                            CloudStorageMananger manager = new CloudStorageMananger();
                            manager.UploadFromStream(ConfigurationManager.AppSettings["AzureAccountName"],
                                ConfigurationManager.AppSettings["AzureAccountKey"],
                                comprobante.Emisor.PublicKey.ToString("N"),
                                comprobante.PublicKey.ToString("N") + ".xml",
                                model.ComprobanteArchivo.FileName,
                                model.ComprobanteArchivo.ContentType,
                                model.ComprobanteArchivo.InputStream);

                            manager.UploadFromStream(ConfigurationManager.AppSettings["AzureAccountName"],
                                ConfigurationManager.AppSettings["AzureAccountKey"],
                                comprobante.Emisor.PublicKey.ToString("N"),
                                comprobante.PublicKey.ToString("N") + ".pdf",
                                model.ComprobantePDFArchivo.FileName,
                                model.ComprobantePDFArchivo.ContentType,
                                model.ComprobantePDFArchivo.InputStream);
                        }
                    }
                }
                catch (Exception ex) {
                    //log.Error(ex, "Error upload photo blob to storage");
                    ex.ToString();
                }
            }
            return RedirectToAction("Details", "Comprobante", new { id = comprobanteId });
        }
    }
}