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
            comprobante.FormaDePago = "PAGO EN UNA SOLA EXHIBICION";
            comprobante.SubTotal = model.SubTotal;
            comprobante.Total = model.Total;

            //comprobante.NoCertificado;
            //comprobante.Certificado;
            comprobante.TipoDeComprobante = "ingreso";

            comprobante.MetodoDePago = "NO IDENTIFICADO";
            comprobante.LugarExpedicion = "MATRIZ";
            comprobante.TipoCambio = "1.00";



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
                comprobante.Impuestos.TotalImpuestosTrasladados = model.IVA;
                comprobante.Impuestos.Traslados = new List<Traslado>();
                comprobante.Impuestos.Traslados.Add(new Traslado {
                    Impuesto = "IVA",
                    Tasa = model.TasaIVA, //  16.00M,
                    Importe = model.IVA
                });
            }

            return View(model);
        }

        public ActionResult Details(string id) {
            Guid publicKey;
            if (!Guid.TryParse(id, out publicKey))
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var comprobante = DBContext.Comprobantes.Where(e => e.PublicKey == publicKey).SingleOrDefault();

            if (comprobante == null)
                return HttpNotFound();

            var model = new ComprbanteDetailViewModel(comprobante);
            return View(model);
        }
    }
}