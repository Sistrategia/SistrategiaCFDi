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
    public class EmisorController : BaseController
    {
        public ActionResult Index() {
            var model = new EmisorIndexViewModel {
                Emisores = this.DBContext.Emisores.ToList()
            };
            return View(model);
        }

        public ActionResult Create() {
            var model = new EmisorCreateViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(EmisorCreateViewModel model) {
            //var model = new EmisorCreateViewModel();
            var emisor = new Emisor();

            emisor.RFC = model.RFC;

            if (!string.IsNullOrEmpty(model.Nombre))
                emisor.Nombre = model.Nombre;

            if (!string.IsNullOrEmpty(model.Pais)) {
                emisor.DomicilioFiscal = new UbicacionFiscal {
                    Pais = model.Pais,
                    Calle = string.IsNullOrEmpty(model.Calle) ? null : model.Calle,
                    NoExterior = string.IsNullOrEmpty(model.NoExterior) ? null : model.NoExterior,
                    NoInterior = string.IsNullOrEmpty(model.NoInterior) ? null : model.NoInterior,
                    Colonia = string.IsNullOrEmpty(model.Colonia) ? null : model.Colonia,
                    Localidad = string.IsNullOrEmpty(model.Localidad) ? null : model.Localidad,
                    Municipio = string.IsNullOrEmpty(model.Municipio) ? null : model.Municipio,
                    Estado = string.IsNullOrEmpty(model.Estado) ? null : model.Estado,
                    CodigoPostal = string.IsNullOrEmpty(model.CodigoPostal) ? null : model.CodigoPostal,
                    Referencia = string.IsNullOrEmpty(model.Referencia) ? null : model.Referencia
                };
            }

            if (!string.IsNullOrEmpty(model.ExpedidoEnPais)) {
                emisor.ExpedidoEn = new Ubicacion {
                    Pais = model.ExpedidoEnPais,
                    Calle = string.IsNullOrEmpty(model.ExpedidoEnCalle) ? null : model.ExpedidoEnCalle,
                    NoExterior = string.IsNullOrEmpty(model.ExpedidoEnNoExterior) ? null : model.ExpedidoEnNoExterior,
                    NoInterior = string.IsNullOrEmpty(model.ExpedidoEnNoInterior) ? null : model.ExpedidoEnNoInterior,
                    Colonia = string.IsNullOrEmpty(model.ExpedidoEnColonia) ? null : model.ExpedidoEnColonia,
                    Localidad = string.IsNullOrEmpty(model.ExpedidoEnLocalidad) ? null : model.ExpedidoEnLocalidad,
                    Municipio = string.IsNullOrEmpty(model.ExpedidoEnMunicipio) ? null : model.ExpedidoEnMunicipio,
                    Estado = string.IsNullOrEmpty(model.ExpedidoEnEstado) ? null : model.ExpedidoEnEstado,
                    CodigoPostal = string.IsNullOrEmpty(model.ExpedidoEnCodigoPostal) ? null : model.ExpedidoEnCodigoPostal,
                    Referencia = string.IsNullOrEmpty(model.ExpedidoEnReferencia) ? null : model.ExpedidoEnReferencia
                };
            }
            //    emisor.DomicilioFiscal.Calle = model.Calle;

            this.DBContext.Emisores.Add(emisor);
            this.DBContext.SaveChanges();

            return View(model);
        }

        public ActionResult Detail(string id) {
            Guid publicKey;
            if (!Guid.TryParse(id, out publicKey))
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var emisor = DBContext.Emisores.Where(e => e.PublicKey == publicKey).SingleOrDefault();

            if (emisor==null)
                return HttpNotFound();

            var model = new EmisorDetailViewModel(emisor);
            return View(model);
        }

        public ActionResult Details(string id) {
            Guid publicKey;
            if (!Guid.TryParse(id, out publicKey))
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var emisor = DBContext.Emisores.Where(e => e.PublicKey == publicKey).SingleOrDefault();

            if (emisor == null)
                return HttpNotFound();

            var model = new EmisorDetailViewModel(emisor);
            return View(model);
        }

        public ActionResult Edit(string id) {
            Guid publicKey = Guid.Parse(id);
            var emisor = DBContext.Emisores.Where(e => e.PublicKey == publicKey).SingleOrDefault();
            var model = new EmisorEditViewModel(emisor);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(EmisorEditViewModel model) {
            //Guid publicKey = Guid.Parse(id);
            //var emisor = DBContext.Emisores.Where(e => e.PublicKey == publicKey).SingleOrDefault();
            
            return View(model);
        }
    }
}