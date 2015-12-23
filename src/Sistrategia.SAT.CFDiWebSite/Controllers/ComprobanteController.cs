using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistrategia.SAT.CFDiWebSite.Models;
using Sistrategia.SAT.CFDiWebSite.CFDI;

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
                var emisores = DBContext.Emisores.Where(x => x.Nombre.Contains(value) || x.RFC.Contains(value))
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
                var receptores = DBContext.Receptores.Where(x => x.Nombre.Contains(value) || x.RFC.Contains(value))
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
    }
}