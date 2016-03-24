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
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace Sistrategia.SAT.CFDiWebSite.Controllers
{
    [Authorize]
    public class ComprobanteController : BaseController
    {

        #region Index

        public ActionResult Index() {
            var model = new ComprobanteIndexViewModel {
                Comprobantes = this.DBContext.Comprobantes.ToList()
            };
            return View(model);
        }

        #endregion

        #region JsonDataServices

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

        public JsonResult GetCteNumeroByReceptor(int receptorId)
        {
            try
            {

                var CteNumero = this.DBContext.Database.SqlQuery<int?>(@"SELECT TOP(1) [extended_int_value_2] FROM [sat_comprobante] WHERE [comprobante_receptor_id] = @receptorId", new SqlParameter("@receptorId", receptorId)).SingleOrDefault();

                if (CteNumero == null)
                {
                    CteNumero = this.DBContext.Database.SqlQuery<int>("SELECT MAX([extended_int_value_2])+1 FROM [sat_comprobante] WHERE [serie] = 'A'").FirstOrDefault();
                }

                var result = new { resp = true, CteNumero = CteNumero };
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
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
                        //orderByFunc = sl => sl.Receptor.Nombre;
                        orderByFunc = sl => sl.Receptor.Receptor.Nombre;
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
                    //Comprobantes = sortDir == "ASC" ? DBContext.Comprobantes.Where(x => x.Receptor.Nombre.Contains(search)
                    Comprobantes = sortDir == "ASC" ? DBContext.Comprobantes.Where(x => x.Receptor.Receptor.Nombre.Contains(search)
                        || x.Total.ToString().Contains(search)
                        || x.Status.Contains(search)
                        || (x.Serie + x.Folio).Contains(search)
                        ).OrderBy(orderByFunc)
                        .Take(((page - 1) * pageSize) + pageSize)
                        .Skip(((page - 1) * pageSize)).ToList()
                        :
                        //DBContext.Comprobantes.Where(x => x.Receptor.Nombre.Contains(search)
                        DBContext.Comprobantes.Where(x => x.Receptor.Receptor.Nombre.Contains(search)
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
                    //int ComprobantesTotalRows = DBContext.Comprobantes.Where(x => x.Receptor.Nombre.Contains(search)
                    int ComprobantesTotalRows = DBContext.Comprobantes.Where(x => x.Receptor.Receptor.Nombre.Contains(search)
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

        #endregion

        #region Create

        public ActionResult Create() {
            var model = new ComprobanteCreateViewModel();

            var tipoMetodoDePagoList = DBContext.TiposMetodoDePago.ToList();
            var tipoMetodoDePagoSelectList = new List<SelectListItem>();
            foreach (var tipoMetodoDePago in tipoMetodoDePagoList) {
                tipoMetodoDePagoSelectList.Add(new SelectListItem {
                    Value = tipoMetodoDePago.TipoMetodoDePagoValue,
                    Text = tipoMetodoDePago.TipoMetodoDePagoValue
                });
            }
            model.TipoMetodoDePago = tipoMetodoDePagoSelectList;

            var tiposImpuestoRetencionList = DBContext.TiposImpuestoRetencion.ToList();
            var tiposImpuestoRetencionSelectList = new List<SelectListItem>();
            foreach (var tiposImpuestoRetencion in tiposImpuestoRetencionList) {
                tiposImpuestoRetencionSelectList.Add(new SelectListItem {
                    Value = tiposImpuestoRetencion.TipoImpuestoRetencionValue,
                    Text = tiposImpuestoRetencion.TipoImpuestoRetencionValue
                });
            }
            model.TiposImpuestoRetencion = tiposImpuestoRetencionSelectList;

            var tiposImpuestoTrasladoList = DBContext.TiposImpuestoTraslado.ToList();
            var tiposImpuestoTrasladoSelectList = new List<SelectListItem>();
            foreach (var tiposImpuestoTraslado in tiposImpuestoTrasladoList) {
                tiposImpuestoTrasladoSelectList.Add(new SelectListItem {
                    Value = tiposImpuestoTraslado.TipoImpuestoTrasladoValue,
                    Text = tiposImpuestoTraslado.TipoImpuestoTrasladoValue
                });
            }
            model.TiposImpuestoTraslado = tiposImpuestoTrasladoSelectList;

            var tiposFormaDePagoList = DBContext.TiposFormaDePago.ToList();
            var tiposFormaDePagoSelectList = new List<SelectListItem>();
            foreach (var tiposFormaDePago in tiposFormaDePagoList) {
                tiposFormaDePagoSelectList.Add(new SelectListItem {
                    Value = tiposFormaDePago.TipoFormaDePagoValue,
                    Text = tiposFormaDePago.TipoFormaDePagoValue
                });
            }
            model.TiposFormaDePago = tiposFormaDePagoSelectList;

            var bancosList = DBContext.Bancos.ToList();
            var bancosSelectList = new List<SelectListItem>();
            foreach (var banco in bancosList) {
                bancosSelectList.Add(new SelectListItem {
                    Value = banco.NombreCorto,
                    Text = banco.NombreCorto
                });
            }
            model.Bancos = bancosSelectList;

            var monedasList = DBContext.TiposMoneda.ToList();
            var monedasListSelectList = new List<SelectListItem>();
            foreach (var moneda in monedasList) {
                monedasListSelectList.Add(new SelectListItem {
                    Value = moneda.TipoMonedaValue,
                    Text = moneda.TipoMonedaValue
                });
            }
            model.TiposMoneda = monedasListSelectList;


            var tiposDeComprobanteList = DBContext.TiposTipoDeComprobante.ToList();
            var tiposDeComprobanteListSelectList = new List<SelectListItem>();
            foreach (var tipoDeComprobante in tiposDeComprobanteList)
            {
                tiposDeComprobanteListSelectList.Add(new SelectListItem
                {
                    Value = tipoDeComprobante.TipoTipoDeComprobanteValue,
                    Text = tipoDeComprobante.TipoTipoDeComprobanteValue
                });
            }
            model.TiposDeComprobante = tiposDeComprobanteListSelectList;

            var viewTemplatesList = DBContext.ViewTemplates.ToList();
            var viewTemplatesListSelectList = new List<SelectListItem>();
            foreach (var vTemplate in viewTemplatesList)
            {
                viewTemplatesListSelectList.Add(new SelectListItem
                {
                    Value = vTemplate.ViewTemplateId.ToString(),
                    Text = vTemplate.DisplayName
                });
            }
            model.ViewTemplates = viewTemplatesListSelectList;


            Emisor emisor = DBContext.Emisores.SingleOrDefault(x => x.EmisorId == 1);


            model.EmisorId = emisor.EmisorId;
            model.Emisor.Nombre = emisor.Nombre;
            model.Emisor.RFC = emisor.RFC;
            // model.ExpedidoEn = emisor.ExpedidoEn select one?
            model.ExpedidoEn.UbicacionId = null; // default null (same as domicilioFiscal)
            model.CertificadoId = emisor.Certificados.FirstOrDefault(x => x.Estado == "A").CertificadoId;

            model.Folio = this.DBContext.Database.SqlQuery<string>("SELECT CONVERT(NVARCHAR,MAX(CONVERT(INT,[folio]))+1) FROM [sat_comprobante] WHERE [serie] = 'A'").FirstOrDefault();
            model.OrdenNumero = this.DBContext.Database.SqlQuery<int>("SELECT MAX([extended_int_value_1])+1 FROM [sat_comprobante] WHERE [serie] = 'A'").FirstOrDefault();


            var SampleConceptsList = this.DBContext.Database.SqlQuery<SelectListItem>("SELECT [no_identificacion] as Value, [no_identificacion] + ' - ' + [descripcion] as Text FROM [sat_concepto] GROUP BY [no_identificacion], [descripcion] order by [no_identificacion] DESC");
            ViewBag.SampleConcepts = SampleConceptsList;

            return View(model);
        }

        [HttpPost]
        public JsonResult Create(ComprobanteCreateViewModel model) {
            try {
                if (String.IsNullOrEmpty(model.LugarExpedicion))
                    throw new ApplicationException("¡Ingrese el lugar de expedición!");
                else if (string.IsNullOrEmpty(model.TipoDeComprobante))
                    throw new ApplicationException("¡Ingrese el tipo de comprobante!");
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
                else if ((model.MetodoDePago != "EFECTIVO" && model.MetodoDePago != "NO IDENTIFICADO") && (model.NumCtaPago == null || (model.NumCtaPago.Count() > 6 || model.NumCtaPago.Count() < 4)))
                    throw new ApplicationException("¡El valor de NumCtaPago debe contener entre 4 hasta 6 caracteres!");
                else if ((model.MetodoDePago != "EFECTIVO" && model.MetodoDePago != "NO IDENTIFICADO") && (string.IsNullOrEmpty(model.Banco)))
                    throw new ApplicationException("¡Ingrese el banco!");
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

                    Emisor emisor = DBContext.Emisores.Find(model.EmisorId);

                    ComprobanteEmisor comprobanteEmisor = null;

                    if (model.ExpedidoEn != null && model.ExpedidoEn.UbicacionId != null) {
                        comprobanteEmisor = DBContext.ComprobantesEmisores.Where(e => e.EmisorId == emisor.EmisorId && e.DomicilioFiscalId == emisor.DomicilioFiscalId && e.ExpedidoEnId == model.ExpedidoEn.UbicacionId).SingleOrDefault();
                    }
                    //else if () {
                    //}
                    else {
                        // crear o seleccionar la ubicación y agregarla
                        //comprobanteEmisor = DBContext.ComprobantesEmisores.Where(e => e.EmisorId == model.EmisorId && e.DomicilioFiscalId == model.DomicilioFiscalId && e.ExpedidoEnId == model.ExpedidoEnId);
                        comprobanteEmisor = DBContext.ComprobantesEmisores.Where(e => e.EmisorId == emisor.EmisorId && e.DomicilioFiscalId == emisor.DomicilioFiscalId && e.ExpedidoEnId == model.ExpedidoEn.UbicacionId).SingleOrDefault();
                    }

                    // Crear uno nuevo
                    if (comprobanteEmisor == null) {
                        comprobanteEmisor = new ComprobanteEmisor {
                            Emisor = emisor,
                            //EmisorId = emisor.EmisorId,
                            DomicilioFiscal = emisor.DomicilioFiscal
                            //,DomicilioId = receptor.DomicilioId
                            // TODO:
                            //RegimenFiscal = emisor.RegimenFiscal
                        };

                    }

                    comprobante.Emisor = comprobanteEmisor;

                    //comprobante.EmisorId = model.EmisorId;
                    //comprobante.Emisor = DBContext.Emisores.Find(model.EmisorId); // .Where(e => e.PublicKey == publicKey).SingleOrDefault();
                    //if (model.Emisor. .ExpedidoEnId != null) {
                    //    comprobante.Emisor = DBContext.ComprobantesEmisores.Where(e => e.EmisorId == model.EmisorId && e.DomicilioFiscalId == model.DomicilioFiscalId && e.ExpedidoEnId == model.ExpedidoEnId);
                    //}
                    //else {
                    //    comprobante.Emisor = DBContext.ComprobantesEmisores.Where(e => e.EmisorId == model.EmisorId && e.DomicilioFiscalId == model.DomicilioFiscalId && e.ExpedidoEnId == model.ExpedidoEnId);
                    //}

                    Receptor receptor = DBContext.Receptores.Find(model.ReceptorId);

                    ComprobanteReceptor comprobanteReceptor = DBContext.ComprobantesReceptores.Where(r => r.ReceptorId == receptor.ReceptorId && r.DomicilioId == receptor.DomicilioId).SingleOrDefault();

                    // Crear uno nuevo
                    if (comprobanteReceptor == null) {
                        comprobanteReceptor = new ComprobanteReceptor {
                            Receptor = receptor,
                            //ReceptorId = receptor.ReceptorId,
                            Domicilio = receptor.Domicilio
                            //,DomicilioId = receptor.DomicilioId
                        };
                    }


                    //comprobante.ReceptorId = model.ReceptorId;
                    //comprobante.Receptor = DBContext.Receptores.Find(model.ReceptorId); // .Where(e => e.PublicKey == publicKey).SingleOrDefault();
                    comprobante.Receptor = comprobanteReceptor;
                    comprobante.Serie = model.Serie;
                    comprobante.Folio = model.Folio;
                    comprobante.Fecha = DateTime.Now + SATManager.GetCFDIServiceTimeSpan();
                    comprobante.FormaDePago = model.FormaDePago;
                    comprobante.SubTotal = model.SubTotal;
                    comprobante.Total = model.Total;

                    //comprobante.NoCertificado;
                    //comprobante.Certificado;
                    comprobante.TipoDeComprobante = model.TipoDeComprobante;

                    comprobante.ExtendedIntValue1 = model.OrdenNumero;
                    comprobante.ExtendedIntValue2 = model.CteNumero;
                    comprobante.ExtendedStringValue2 = model.Notas;


                    comprobante.ViewTemplateId = model.TemplateId;

                    comprobante.FormaDePago = model.FormaDePago;
                    comprobante.MetodoDePago = model.MetodoDePago;
                    comprobante.LugarExpedicion = model.LugarExpedicion;
                    comprobante.TipoCambio = model.TipoCambio;
                    comprobante.Moneda = model.Moneda;
                    comprobante.NumCtaPago = model.NumCtaPago;
                    comprobante.ExtendedStringValue1 = model.Banco;
                    comprobante.Moneda = model.Moneda;

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
                                PublicKey = Guid.NewGuid(),
                                Ordinal = modelConcepto.Ordinal
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
                    //comprobante.Status = "P";
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
            catch (System.Data.Entity.Validation.DbEntityValidationException dex) {
                string errorTxt = dex.Message.ToString();

                foreach (var valError in dex.EntityValidationErrors)
                {
                    foreach (var error in valError.ValidationErrors)
                    {
                        errorTxt = errorTxt + Environment.NewLine + error.ErrorMessage;
                    }
                }

                var data = new {
                    error = true,
                    errorMsg = errorTxt

                };

                

                return Json(data);
            }
            catch (Exception ex) {

                string errorTxt = ex.Message.ToString();

                foreach (ModelState modelState in ViewData.ModelState.Values)
                {
                    foreach (ModelError error in modelState.Errors)
                    {
                        errorTxt = errorTxt + Environment.NewLine + error.ErrorMessage;
                    }
                }

            

                var data = new {
                    error = true,
                    errorMsg = errorTxt

                };

                

                return Json(data);
            }
        }

        #endregion

        #region Upload

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
                    Emisor comprobanteEmisor = null;// new Emisor();
                    Receptor comprobanteReceptor = null; // new Receptor();
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
                                    //comprobante.Emisor = new Emisor();
                                    comprobanteEmisor = new Emisor();
                                    while (xmlReader.MoveToNextAttribute()) {
                                        switch (xmlReader.Name) {
                                            case "rfc":
                                                //comprobante.Emisor.RFC = xmlReader.Value;
                                                comprobanteEmisor.RFC = xmlReader.Value;
                                                break;
                                            case "nombre":
                                                //comprobante.Emisor.Nombre = xmlReader.Value;
                                                comprobanteEmisor.Nombre = xmlReader.Value;
                                                break;
                                            default:
                                                throw new Exception(xmlReader.Name + "is not a valid attribute for cfdi:Emisor.");
                                        }
                                    }
                                }

                                else if ("cfdi:DomicilioFiscal".Equals(xmlReader.Name)) {
                                    comprobanteEmisor.DomicilioFiscal = new UbicacionFiscal();
                                    while (xmlReader.MoveToNextAttribute()) {
                                        switch (xmlReader.Name) {
                                            case "calle":
                                                comprobanteEmisor.DomicilioFiscal.Calle = xmlReader.Value;
                                                break;
                                            case "noExterior":
                                                comprobanteEmisor.DomicilioFiscal.NoExterior = xmlReader.Value;
                                                break;
                                            case "noInterior":
                                                comprobanteEmisor.DomicilioFiscal.NoInterior = xmlReader.Value;
                                                break;
                                            case "colonia":
                                                comprobanteEmisor.DomicilioFiscal.Colonia = xmlReader.Value;
                                                break;
                                            case "localidad":
                                                comprobanteEmisor.DomicilioFiscal.Localidad = xmlReader.Value;
                                                break;
                                            case "referencia":
                                                comprobanteEmisor.DomicilioFiscal.Referencia = xmlReader.Value;
                                                break;
                                            case "municipio":
                                                comprobanteEmisor.DomicilioFiscal.Municipio = xmlReader.Value;
                                                break;
                                            case "estado":
                                                comprobanteEmisor.DomicilioFiscal.Estado = xmlReader.Value;
                                                break;
                                            case "pais":
                                                comprobanteEmisor.DomicilioFiscal.Pais = xmlReader.Value;
                                                break;
                                            case "codigoPostal":
                                                comprobanteEmisor.DomicilioFiscal.CodigoPostal = xmlReader.Value;
                                                break;
                                            default:
                                                throw new Exception(xmlReader.Name + "is not a valid attribute for cfdi:DomicilioFiscal.");
                                        }
                                    }
                                }

                                else if ("cfdi:RegimenFiscal".Equals(xmlReader.Name)) {
                                    if (comprobanteEmisor.RegimenFiscal == null)
                                        comprobanteEmisor.RegimenFiscal = new List<RegimenFiscal>();
                                    while (xmlReader.MoveToNextAttribute()) {
                                        switch (xmlReader.Name) {
                                            case "Regimen":
                                                RegimenFiscal regimen = new RegimenFiscal();
                                                regimen.Regimen = xmlReader.Value;
                                                comprobanteEmisor.RegimenFiscal.Add(regimen);
                                                break;
                                            default:
                                                throw new Exception(xmlReader.Name + "is not a valid attribute for cfdi:RegimenFiscal.");
                                        }
                                    }
                                }

                                else if ("cfdi:Receptor".Equals(xmlReader.Name)) {
                                    //comprobante.Receptor = new Receptor();
                                    comprobanteReceptor = new Receptor();
                                    while (xmlReader.MoveToNextAttribute()) {
                                        switch (xmlReader.Name) {
                                            case "rfc":
                                                //comprobante.Receptor.RFC = xmlReader.Value;
                                                comprobanteReceptor.RFC = xmlReader.Value;
                                                break;
                                            case "nombre":
                                                //comprobante.Receptor.Nombre = xmlReader.Value;
                                                comprobanteReceptor.Nombre = xmlReader.Value;
                                                break;
                                            default:
                                                throw new Exception(xmlReader.Name + "is not a valid attribute for cfdi:Receptor.");
                                        }
                                    }
                                }

                                else if ("cfdi:Domicilio".Equals(xmlReader.Name)) {
                                    //comprobante.Receptor.Domicilio = new Ubicacion();
                                    comprobanteReceptor.Domicilio = new Ubicacion();
                                    while (xmlReader.MoveToNextAttribute()) {
                                        switch (xmlReader.Name) {
                                            case "calle":
                                                comprobanteReceptor.Domicilio.Calle = xmlReader.Value;
                                                break;
                                            case "noExterior":
                                                comprobanteReceptor.Domicilio.NoExterior = xmlReader.Value;
                                                break;
                                            case "noInterior":
                                                comprobanteReceptor.Domicilio.NoInterior = xmlReader.Value;
                                                break;
                                            case "colonia":
                                                comprobanteReceptor.Domicilio.Colonia = xmlReader.Value;
                                                break;
                                            case "localidad":
                                                comprobanteReceptor.Domicilio.Localidad = xmlReader.Value;
                                                break;
                                            case "referencia":
                                                comprobanteReceptor.Domicilio.Referencia = xmlReader.Value;
                                                break;
                                            case "municipio":
                                                comprobanteReceptor.Domicilio.Municipio = xmlReader.Value;
                                                break;
                                            case "estado":
                                                comprobanteReceptor.Domicilio.Estado = xmlReader.Value;
                                                break;
                                            case "pais":
                                                comprobanteReceptor.Domicilio.Pais = xmlReader.Value;
                                                break;
                                            case "codigoPostal":
                                                comprobanteReceptor.Domicilio.CodigoPostal = xmlReader.Value;
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

                        if (comprobanteEmisor != null) {
                            //Emisor emisor = DBContext.Emisores.Where(e => 
                            //    (e.RFC == comprobante.Emisor.RFC)
                            //    && (e.Nombre == comprobante.Emisor.Nombre)                                
                            //    && (e.Status == "A")
                            //    ).SingleOrDefault();

                            List<Emisor> emisores = DBContext.Emisores.Where(e =>
                                (e.RFC == comprobanteEmisor.RFC)
                                && (e.Nombre == comprobanteEmisor.Nombre)
                                ).ToList();

                            if (emisores != null && emisores.Count > 0) {
                                foreach (Emisor emisor in emisores) {

                                    if ((emisor.DomicilioFiscal != null && comprobanteEmisor.DomicilioFiscal != null)
                                        && (emisor.DomicilioFiscal.Calle == comprobanteEmisor.DomicilioFiscal.Calle)
                                        && (emisor.DomicilioFiscal.NoExterior == comprobanteEmisor.DomicilioFiscal.NoExterior)
                                        && (emisor.DomicilioFiscal.NoInterior == comprobanteEmisor.DomicilioFiscal.NoInterior)
                                        && (emisor.DomicilioFiscal.Colonia == comprobanteEmisor.DomicilioFiscal.Colonia)
                                        && (emisor.DomicilioFiscal.Referencia == comprobanteEmisor.DomicilioFiscal.Referencia)
                                        && (emisor.DomicilioFiscal.Localidad == comprobanteEmisor.DomicilioFiscal.Localidad)
                                        && (emisor.DomicilioFiscal.Municipio == comprobanteEmisor.DomicilioFiscal.Municipio)
                                        && (emisor.DomicilioFiscal.Estado == comprobanteEmisor.DomicilioFiscal.Estado)
                                        && (emisor.DomicilioFiscal.CodigoPostal == comprobanteEmisor.DomicilioFiscal.CodigoPostal)
                                        && (emisor.DomicilioFiscal.Pais == comprobanteEmisor.DomicilioFiscal.Pais)
                                        ) {

                                        //if (receptor != null) {
                                        comprobanteEmisor = emisor;
                                        comprobanteEmisor.EmisorId = emisor.EmisorId;
                                        comprobanteEmisor.DomicilioFiscal = emisor.DomicilioFiscal;
                                        comprobanteEmisor.DomicilioFiscalId = emisor.DomicilioFiscalId;
                                        //comprobante.Emisor = DBContext.ComprobantesEmisores.Where( e => e.EmisorId == emisor.EmisorId && e.DomicilioFiscalId = emisor.DomicilioFiscalId && e.ExpedidoEnId)
                                        comprobante.Emisor = DBContext.ComprobantesEmisores.Where(e => e.EmisorId == emisor.EmisorId && e.DomicilioFiscalId == emisor.DomicilioFiscalId).SingleOrDefault();

                                        if (comprobante.Emisor == null) {
                                            comprobante.Emisor = new ComprobanteEmisor {
                                                Emisor = comprobanteEmisor,
                                                //EmisorId = emisor.EmisorId,
                                                DomicilioFiscal = comprobanteEmisor.DomicilioFiscal
                                                //,DomicilioId = receptor.DomicilioId
                                                // TODO:
                                                //RegimenFiscal = emisor.RegimenFiscal
                                            };
                                        }
                                    }
                                }
                                //if (comprobanteEmisor.DomicilioFiscalId == null) {
                                //    // The address has changed, create a new one and inactive the oldone
                                //    foreach (Emisor emisor in emisores) {
                                //        emisor.Status = "I";
                                //    }
                                //    comprobante.Emisor.Status = "A";

                                //}
                                if (comprobanteEmisor.EmisorId == null) {
                                    // The address has changed, create a new one and inactive the oldone

                                    foreach (Emisor emisor in emisores) {
                                        emisor.Status = "I";
                                    }

                                    comprobanteEmisor.Status = "A";

                                    comprobante.Emisor = new ComprobanteEmisor {
                                        Emisor = comprobanteEmisor,
                                        //EmisorId = emisor.EmisorId,
                                        DomicilioFiscal = comprobanteEmisor.DomicilioFiscal
                                        //,DomicilioId = receptor.DomicilioId
                                        // TODO:
                                        //RegimenFiscal = emisor.RegimenFiscal
                                    };

                                }
                                

                            }
                            else {
                                comprobante.Emisor = new ComprobanteEmisor {
                                    Emisor = comprobanteEmisor,
                                    //EmisorId = emisor.EmisorId,
                                    DomicilioFiscal = comprobanteEmisor.DomicilioFiscal
                                    //,DomicilioId = receptor.DomicilioId
                                    // TODO:
                                    //RegimenFiscal = emisor.RegimenFiscal
                                };
                                comprobanteEmisor.Status = "A";
                            }
                        }

                        if (comprobanteReceptor != null) {
                            //Receptor receptor = DBContext.Receptores.Where(r =>
                            //    (r.RFC == comprobante.Receptor.RFC)
                            //    && (r.Nombre == comprobante.Receptor.Nombre)
                            //    && (r.Status == "A")
                            //    ).SingleOrDefault();

                            List<Receptor> receptores = DBContext.Receptores.Where(r =>
                                (r.RFC == comprobanteReceptor.RFC)
                                && (r.Nombre == comprobanteReceptor.Nombre)
                                ).ToList();

                            if (receptores != null && receptores.Count > 0) {
                                foreach (Receptor receptor in receptores) {

                                    if ((receptor.Domicilio != null && comprobanteReceptor.Domicilio != null)
                                        && (receptor.Domicilio.Calle == comprobanteReceptor.Domicilio.Calle)
                                        && (receptor.Domicilio.NoExterior == comprobanteReceptor.Domicilio.NoExterior)
                                        && (receptor.Domicilio.NoInterior == comprobanteReceptor.Domicilio.NoInterior)
                                        && (receptor.Domicilio.Colonia == comprobanteReceptor.Domicilio.Colonia)
                                        && (receptor.Domicilio.Referencia == comprobanteReceptor.Domicilio.Referencia)
                                        && (receptor.Domicilio.Localidad == comprobanteReceptor.Domicilio.Localidad)
                                        && (receptor.Domicilio.Municipio == comprobanteReceptor.Domicilio.Municipio)
                                        && (receptor.Domicilio.Estado == comprobanteReceptor.Domicilio.Estado)
                                        && (receptor.Domicilio.CodigoPostal == comprobanteReceptor.Domicilio.CodigoPostal)
                                        && (receptor.Domicilio.Pais == comprobanteReceptor.Domicilio.Pais)
                                        ) {

                                        //if (receptor != null) {
                                        comprobanteReceptor = receptor;
                                        comprobanteReceptor.ReceptorId = receptor.ReceptorId;
                                        comprobanteReceptor.Domicilio = receptor.Domicilio;
                                        comprobanteReceptor.DomicilioId = receptor.DomicilioId;

                                        comprobante.Receptor = DBContext.ComprobantesReceptores.Where(r => r.ReceptorId == receptor.ReceptorId && r.DomicilioId == receptor.DomicilioId).SingleOrDefault();

                                        if (comprobante.Receptor == null) {
                                            comprobante.Receptor = new ComprobanteReceptor {
                                                Receptor = comprobanteReceptor,
                                                //ReceptorId = comprobanteReceptor.ReceptorId,
                                                Domicilio = comprobanteReceptor.Domicilio
                                                //,DomicilioId = comprobanteReceptor.DomicilioId
                                            };
                                        }
                                    }
                                }
                                if (comprobanteReceptor.ReceptorId == null) {
                                    // The address has changed, create a new one and inactive the oldone

                                    foreach (Receptor receptor in receptores) {
                                        receptor.Status = "I";
                                    }

                                    comprobanteReceptor.Status = "A";

                                    comprobante.Receptor = new ComprobanteReceptor {
                                        Receptor = comprobanteReceptor,
                                        //ReceptorId = comprobanteReceptor.ReceptorId,
                                        Domicilio = comprobanteReceptor.Domicilio
                                        //,DomicilioId = comprobanteReceptor.DomicilioId
                                    };

                                }
                            }
                            else {
                                comprobanteReceptor.Status = "A";
                                comprobante.Receptor = new ComprobanteReceptor {
                                    Receptor = comprobanteReceptor,
                                    //ReceptorId = comprobanteReceptor.ReceptorId,
                                    Domicilio = comprobanteReceptor.Domicilio
                                    //,DomicilioId = comprobanteReceptor.DomicilioId
                                };
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
                        if (comprobante.Receptor.ReceptorId != null && comprobante.Receptor.ReceptorId > 0)
                            comprobante.ExtendedIntValue2 = comprobante.Receptor.ReceptorId;
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

        #endregion

        #region Edit

        public ActionResult Edit(string id) {
            Guid publicKey = Guid.Parse(id);
            var comprobante = DBContext.Comprobantes.Where(e => e.PublicKey == publicKey && e.Status == "P")
                .SingleOrDefault();
            var model = new ComprobanteEditViewModel(comprobante);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ComprobanteEditViewModel model) {
            var comprobanteId = model.ComprobanteId; // Guid.Parse(model.ComprobanteId);
            var comprobante = DBContext.Comprobantes.Find(comprobanteId);
            //var comprobante = DBContext.Comprobantes.Where(e => e.PublicKey == publicKey && e.Status == "P")
            //    .SingleOrDefault();       

            comprobante.Folio = model.Folio;
            comprobante.Fecha = DateTime.ParseExact(model.Fecha, "yyyy-MM-ddTHH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeLocal);

            comprobante.ExtendedStringValue2 = model.Notas;

            var certificado = DBContext.Certificados.Where(e => e.NumSerie == comprobante.NoCertificado).SingleOrDefault();
            comprobante.Sello = certificado.GetSello(comprobante.GetCadenaOriginal());

            DBContext.SaveChanges();

            model = new ComprobanteEditViewModel(comprobante);
            return View(model);
        }

        //public ActionResult EditOLD(string id) {
        //    Guid publicKey = Guid.Parse(id);
        //    var comprobante = DBContext.Comprobantes.Where(e => e.PublicKey == publicKey && e.Status == "P")
        //        .SingleOrDefault();
        //    var model = new ComprobanteEditViewModel(comprobante);

        //    var tipoMetodoDePagoList = DBContext.TiposMetodoDePago.ToList();
        //    var tipoMetodoDePagoSelectList = new List<SelectListItem>();
        //    foreach (var tipoMetodoDePago in tipoMetodoDePagoList) {
        //        tipoMetodoDePagoSelectList.Add(new SelectListItem {
        //            Value = tipoMetodoDePago.TipoMetodoDePagoValue,
        //            Text = tipoMetodoDePago.TipoMetodoDePagoValue
        //        });
        //    }
        //    model.TipoMetodoDePago = tipoMetodoDePagoSelectList;

        //    var tiposFormaDePagoList = DBContext.TiposFormaDePago.ToList();
        //    var tiposFormaDePagoListSelectList = new List<SelectListItem>();
        //    foreach (var formaDePago in tiposFormaDePagoList) {
        //        tiposFormaDePagoListSelectList.Add(new SelectListItem {
        //            Value = formaDePago.TipoFormaDePagoValue,
        //            Text = formaDePago.TipoFormaDePagoValue
        //        });
        //    }
        //    model.TiposFormaDePago = tiposFormaDePagoListSelectList;

        //    var tiposImpuestoRetencionList = DBContext.TiposImpuestoRetencion.ToList();
        //    var tiposImpuestoRetencionSelectList = new List<SelectListItem>();
        //    foreach (var tiposImpuestoRetencion in tiposImpuestoRetencionList) {
        //        tiposImpuestoRetencionSelectList.Add(new SelectListItem {
        //            Value = tiposImpuestoRetencion.TipoImpuestoRetencionValue,
        //            Text = tiposImpuestoRetencion.TipoImpuestoRetencionValue
        //        });
        //    }
        //    model.TiposImpuestoRetencion = tiposImpuestoRetencionSelectList;

        //    var tiposImpuestoTrasladoList = DBContext.TiposImpuestoTraslado.ToList();
        //    var tiposImpuestoTrasladoSelectList = new List<SelectListItem>();
        //    foreach (var tiposImpuestoTraslado in tiposImpuestoTrasladoList) {
        //        tiposImpuestoTrasladoSelectList.Add(new SelectListItem {
        //            Value = tiposImpuestoTraslado.TipoImpuestoTrasladoValue,
        //            Text = tiposImpuestoTraslado.TipoImpuestoTrasladoValue
        //        });
        //    }
        //    model.TiposImpuestoTraslado = tiposImpuestoTrasladoSelectList;

        //    var tiposMonedaList = DBContext.TiposMoneda.ToList();
        //    var tiposMonedaListSelectList = new List<SelectListItem>();
        //    foreach (var moneda in tiposMonedaList) {
        //        tiposMonedaListSelectList.Add(new SelectListItem {
        //            Value = moneda.TipoMonedaValue,
        //            Text = moneda.TipoMonedaValue
        //        });
        //    }
        //    model.TiposMoneda = tiposMonedaListSelectList;

        //    List<dynamic> itemList = new List<dynamic>();
        //    foreach (ConceptoViewModel concepto in model.Conceptos) {
        //        var dynamicItems = new {
        //            ConceptoCantidad = concepto.Cantidad,
        //            ConceptoUnidad = concepto.Unidad,
        //            ConceptoNoIdentificacion = concepto.NoIdentificacion,
        //            ConceptoDescripcion = concepto.Descripcion,
        //            ConceptoValorUnitario = concepto.ValorUnitario,
        //            ConceptoImporte = concepto.Importe,
        //            ConceptoOrdinal = concepto.Ordinal
        //        };

        //        itemList.Add(dynamicItems);
        //    }

        //    ViewBag.JsonConceptos = JsonConvert.SerializeObject(itemList);
        //    ViewBag.TotalConceptos = itemList.Count();

        //    List<dynamic> itemList2 = new List<dynamic>();

        //    if (model.Traslados != null && model.Traslados.Count > 0) {

        //        int ordinal = 0;
        //        foreach (TrasladoViewModel traslado in model.Traslados) {
        //            ordinal++;
        //            var dynamicItems2 = new {
        //                TrasladadoImpuesto = traslado.Impuesto,
        //                TrasladadoTasa = traslado.Tasa,
        //                TrasladadoImporte = traslado.Importe,
        //                TrasladadoOrdinal = ordinal
        //            };
        //            itemList2.Add(dynamicItems2);
        //        }

        //        ViewBag.JsonImpuestosTrasladados = JsonConvert.SerializeObject(itemList2);
        //        ViewBag.TotalImpuestosTrasladados = model.Traslados.Count();
        //    }
        //    else {
        //        ViewBag.JsonImpuestosTrasladados = JsonConvert.SerializeObject(itemList2);
        //        ViewBag.TotalImpuestosTrasladados = 0;
        //    }

        //    List<dynamic> itemList3 = new List<dynamic>();

        //    if (model.Retenciones != null && model.Retenciones.Count > 0) {

        //        int ordinal2 = 0;
        //        foreach (RetencionViewModel retencion in model.Retenciones) {
        //            ordinal2++;
        //            var dynamicItems3 = new {
        //                RetencionImpuesto = retencion.Impuesto,
        //                RetencionImporte = retencion.Importe,
        //                RetencionOrdinal = ordinal2
        //            };
        //            itemList3.Add(dynamicItems3);
        //        }

        //        ViewBag.JsonImpuestosRetenidos = JsonConvert.SerializeObject(itemList3);
        //        ViewBag.TotalImpuestosRetenidos = model.Retenciones.Count();
        //    }
        //    else {
        //        ViewBag.JsonImpuestosRetenidos = JsonConvert.SerializeObject(itemList3);
        //        ViewBag.TotalImpuestosRetenidos = 0;
        //    }

        //    return View(model);
        //}

        #endregion

        #region Details

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

        #endregion

        #region GetTimbre

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

            string invoiceFileName = DateTime.Now.ToString("yyyyMMddHHmmss_" + comprobante.PublicKey.ToString("N"));
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

                return RedirectToAction("Details", "Comprobante", new { id = comprobante.PublicKey.ToString() });
            }
            catch (Exception ex) {
                TempData["error"] = ex.Message.ToString();
                return View(model);
                //    return View();
            }




            return View(model);
        }

        #endregion

        #region ShowHtml

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

        #endregion

        #region Cancel

        public ActionResult Cancel(string id) {
            //Guid publicKey;
            //if (!Guid.TryParse(id, out publicKey))
            //    return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);




            //var comprobante = DBContext.Comprobantes.Where(e => e.PublicKey == publicKey).SingleOrDefault();

            //if (comprobante == null)
            //    return HttpNotFound();


            //var certificado = DBContext.Certificados.Where(e => e.NumSerie == comprobante.NoCertificado).SingleOrDefault();
            //string[] UUIDs = new string[1];
            //UUIDs[0] = ((TimbreFiscalDigital)comprobante.Complementos[0]).UUID;

            //string user = ConfigurationManager.AppSettings["CfdiServiceUser"];
            //string password = ConfigurationManager.AppSettings["CfdiServicePassword"];

            ////var model = new ComprobanteDetailViewModel(comprobante);

            //string invoiceFileName = DateTime.Now.ToString("cancelado_yyyyMMddHHmmss_" + comprobante.PublicKey.ToString("N") + ".txt");

            //try {

            //    SATManager manager = new SATManager();



            //  //  ICancelaResponse response = manager.CancelaCFDI(user, password, comprobante.Emisor.RFC, UUIDs, certificado.PFXArchivo, certificado.PFXContrasena);



            //    // response.Ack.ToString();
            //    //if (response)
            //    //    DBContext.SaveChanges();


            //}
            //catch (Exception ex) {
            //    TempData["msg"] = ex.Message.ToString();
            //    return View();
            //    //return View(model);
            //    //    return View();
            //}




            return View();

        }

        #endregion

        #region UploadCancelacion

        public ActionResult UploadCancelacion() {
            var model = new ComprobanteUploadCancelacionViewModel();
            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult UploadCancelacion(ComprobanteUploadCancelacionViewModel model) {
            string comprobanteId = "";
            if (ModelState.IsValid) {



                if (model.CancelacionArchivo == null || model.CancelacionArchivo.ContentLength == 0) {
                    return View();
                }
                try {

                    //Comprobante comprobante = new Comprobante();
                    //Certificado certificado = new Certificado();

                    if (model.CancelacionArchivo != null) {
                        // MemoryStream target = new MemoryStream();

                        //model.CancelacionArchivo.ContentType.ToString();
                        if (model.CancelacionArchivo.ContentType == "text/plain") {
                            BinaryReader b = new BinaryReader(model.CancelacionArchivo.InputStream);
                            byte[] binData = b.ReadBytes(model.CancelacionArchivo.ContentLength);

                            string result = System.Text.Encoding.UTF8.GetString(binData);
                            const string CANCELACION_DE_UUID = "CANCELACION DE UUID: ";
                            if (result.StartsWith(CANCELACION_DE_UUID)) {
                                Guid uuid = new Guid(result.Substring(CANCELACION_DE_UUID.Length, 36));

                                string base64EncodedData =
                                    result.Substring(result.IndexOf("[ack] => ") + "[ack] => ".Length,
                                            result.IndexOf("\r\n", result.IndexOf("[ack] => ") + "[ack] => ".Length) - result.IndexOf("[ack] => ") - "[ack] => ".Length);


                                var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
                                string uncodedData = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);

                                //System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
                                //doc.LoadXml(uncodedData);

                                //var comprobante = DBContext.Comprobantes
                                //    .Select(c => new {
                                //        Comprobante = c,
                                //        Complemento = c.Complementos.OfType<TimbreFiscalDigital>()
                                //        .Where(t => t.UUID == uuid.ToString()
                                //        ).Where(t => t.UUID == uuid.ToString())  //.Where(co => co is TimbreFiscalDigital && ((TimbreFiscalDigital)co).UUID == uuid.ToString()).OfType<TimbreFiscalDigital>()
                                //    }
                                //    );

                                string q = "SELECT c.comprobante_id FROM sat_comprobante as c INNER JOIN sat_complemento as co on (c.comprobante_id = co.comprobante_id) INNER JOIN sat_timbre_fiscal_digital as t ON (co.complemento_id = t.complemento_id) WHERE t.uuid = @uuid";


                                //var comprobante = DBContext.Comprobantes.SqlQuery(q, id).SingleOrDefaultAsync();

                                int ccomprobanteId = DBContext.Database.SqlQuery<int>(q, new System.Data.SqlClient.SqlParameter("@uuid", uuid)).FirstOrDefault(); // .FirstOrDefault<int>();

                                var comprobante = DBContext.Comprobantes.Find(ccomprobanteId); // DBContext.Comprobantes.SqlQuery(q).SingleOrDefault();

                                // var comprobante = DBContext.Comprobantes.SqlQuery(q, new System.Data.SqlClient.SqlParameter("@uuid", uuid)).FirstOrDefault(); // .FirstOrDefault<int>();

                                //var comprobante = DBContext.Comprobantes
                                //    .Include(c => c.Complementos.Select(t => t.TimbreFiscalDigital))
                                //    //.Include("Complemento.TimbreFiscalDigital")                         
                                //    .Select(c => new {
                                //    Comprobante = c,
                                //    //Complemento = c.Complementos.Where(co => ((TimbreFiscalDigital)co).UUID == uuid.ToString())
                                //    TimbreFiscalDigital = c.Complementos.Where(co => ((TimbreFiscalDigital)co).UUID == uuid.ToString()).OfType<TimbreFiscalDigital>()
                                //});
                                if (comprobante != null) {
                                    comprobante.ToString();
                                }

                                //.Include(c => c.Complementos)
                                //.Where(c => c.Complementos.Any(co => ((TimbreFiscalDigital)co).UUID == uuid.ToString()));


                                //var query = from c in DBContext.Comprobantes
                                //            from co in c.Complementos
                                //            where ((TimbreFiscalDigital)co).UUID == uuid.ToString()
                                //            select c;





                                //if (model.ComprobantePDFArchivo != null && model.ComprobantePDFArchivo.ContentLength > 0) {
                                //    CloudStorageMananger manager = new CloudStorageMananger();
                                //    manager.UploadFromStream(ConfigurationManager.AppSettings["AzureAccountName"],
                                //        ConfigurationManager.AppSettings["AzureAccountKey"],
                                //        comprobante.Emisor.PublicKey.ToString("N"),
                                //        comprobante.PublicKey.ToString("N") + ".xml",
                                //        model.ComprobanteArchivo.FileName,
                                //        model.ComprobanteArchivo.ContentType,
                                //        model.ComprobanteArchivo.InputStream);

                                //    manager.UploadFromStream(ConfigurationManager.AppSettings["AzureAccountName"],
                                //        ConfigurationManager.AppSettings["AzureAccountKey"],
                                //        comprobante.Emisor.PublicKey.ToString("N"),
                                //        comprobante.PublicKey.ToString("N") + ".pdf",
                                //        model.ComprobantePDFArchivo.FileName,
                                //        model.ComprobantePDFArchivo.ContentType,
                                //        model.ComprobantePDFArchivo.InputStream);
                                //}


                            }
                        }

                        //System.Xml.XmlTextReader xmlReader = new System.Xml.XmlTextReader(model.CancelacionArchivo.InputStream);
                    }
                }
                catch (Exception ex) {
                    //log.Error(ex, "Error upload photo blob to storage");
                    ex.ToString();
                }

                //return RedirectToAction("Details", "Comprobante", new { id = comprobanteId });
                //return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }



        #endregion





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