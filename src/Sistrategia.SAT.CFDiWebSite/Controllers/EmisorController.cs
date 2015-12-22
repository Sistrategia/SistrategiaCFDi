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

            if (model.DomicilioFiscal != null) {
                if (!string.IsNullOrEmpty(model.DomicilioFiscal.Pais)) {
                    emisor.DomicilioFiscal = new UbicacionFiscal {
                        Pais = model.DomicilioFiscal.Pais,
                        Calle = string.IsNullOrEmpty(model.DomicilioFiscal.Calle) ? null : model.DomicilioFiscal.Calle,
                        NoExterior = string.IsNullOrEmpty(model.DomicilioFiscal.NoExterior) ? null : model.DomicilioFiscal.NoExterior,
                        NoInterior = string.IsNullOrEmpty(model.DomicilioFiscal.NoInterior) ? null : model.DomicilioFiscal.NoInterior,
                        Colonia = string.IsNullOrEmpty(model.DomicilioFiscal.Colonia) ? null : model.DomicilioFiscal.Colonia,
                        Localidad = string.IsNullOrEmpty(model.DomicilioFiscal.Localidad) ? null : model.DomicilioFiscal.Localidad,
                        Municipio = string.IsNullOrEmpty(model.DomicilioFiscal.Municipio) ? null : model.DomicilioFiscal.Municipio,
                        Estado = string.IsNullOrEmpty(model.DomicilioFiscal.Estado) ? null : model.DomicilioFiscal.Estado,
                        CodigoPostal = string.IsNullOrEmpty(model.DomicilioFiscal.CodigoPostal) ? null : model.DomicilioFiscal.CodigoPostal,
                        Referencia = string.IsNullOrEmpty(model.DomicilioFiscal.Referencia) ? null : model.DomicilioFiscal.Referencia
                    };
                }
            }

            if (model.ExpedidoEn != null) {
                if (!string.IsNullOrEmpty(model.ExpedidoEn.Pais)) {
                    emisor.ExpedidoEn = new Ubicacion {
                        Pais = model.ExpedidoEn.Pais,
                        Calle = string.IsNullOrEmpty(model.ExpedidoEn.Calle) ? null : model.ExpedidoEn.Calle,
                        NoExterior = string.IsNullOrEmpty(model.ExpedidoEn.NoExterior) ? null : model.ExpedidoEn.NoExterior,
                        NoInterior = string.IsNullOrEmpty(model.ExpedidoEn.NoInterior) ? null : model.ExpedidoEn.NoInterior,
                        Colonia = string.IsNullOrEmpty(model.ExpedidoEn.Colonia) ? null : model.ExpedidoEn.Colonia,
                        Localidad = string.IsNullOrEmpty(model.ExpedidoEn.Localidad) ? null : model.ExpedidoEn.Localidad,
                        Municipio = string.IsNullOrEmpty(model.ExpedidoEn.Municipio) ? null : model.ExpedidoEn.Municipio,
                        Estado = string.IsNullOrEmpty(model.ExpedidoEn.Estado) ? null : model.ExpedidoEn.Estado,
                        CodigoPostal = string.IsNullOrEmpty(model.ExpedidoEn.CodigoPostal) ? null : model.ExpedidoEn.CodigoPostal,
                        Referencia = string.IsNullOrEmpty(model.ExpedidoEn.Referencia) ? null : model.ExpedidoEn.Referencia
                    };
                }
            }            

            if (!string.IsNullOrEmpty(model.RegimenFiscal)) 
                emisor.RegimenFiscal = new List<RegimenFiscal> {
                    new RegimenFiscal {
                        Regimen = model.RegimenFiscal
                    }
                };

            emisor.Telefono = string.IsNullOrWhiteSpace(model.Telefono) ? null : model.Telefono;
            emisor.Correo = string.IsNullOrWhiteSpace(model.Correo) ? null : model.Correo;

            emisor.CifUrl = string.IsNullOrWhiteSpace(model.CifUrl) ? null : model.CifUrl;
            emisor.LogoUrl = string.IsNullOrWhiteSpace(model.LogoUrl) ? null : model.LogoUrl;

            //if (!string.IsNullOrEmpty(model.RegimenFiscal)) {
            //    if (emisor.RegimenFiscal == null)
            //        emisor.RegimenFiscal = new List<RegimenFiscal>();
            //    emisor.RegimenFiscal.Add(new RegimenFiscal {
            //        Regimen = model.RegimenFiscal
            //    });
            //}

            this.DBContext.Emisores.Add(emisor);
            this.DBContext.SaveChanges();

            return RedirectToAction("Details", new { Id = emisor.PublicKey.ToString("N") }); // "Index", "Home");

            //return View(model);
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
            if (ModelState.IsValid) {
                var emisor = DBContext.Emisores.Where(e => e.PublicKey == model.PublicKey).SingleOrDefault();
                if (emisor != null) {
                    emisor.RFC = model.RFC;
                    emisor.Nombre = model.Nombre;
                    //emisor.RegimenFiscal

                    emisor.Telefono = string.IsNullOrWhiteSpace(model.Telefono) ? null : model.Telefono;
                    emisor.Correo = string.IsNullOrWhiteSpace(model.Correo) ? null : model.Correo;

                    emisor.CifUrl = string.IsNullOrWhiteSpace(model.CifUrl) ? null : model.CifUrl;
                    emisor.LogoUrl = string.IsNullOrWhiteSpace(model.LogoUrl) ? null : model.LogoUrl;
                }

                DBContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }
    }
}