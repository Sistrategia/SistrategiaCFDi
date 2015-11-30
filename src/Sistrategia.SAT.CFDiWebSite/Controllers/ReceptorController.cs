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
    public class ReceptorController : BaseController
    {
        public ActionResult Index() {
            var model = new ReceptorIndexViewModel {
                Receptores = this.DBContext.Receptores.ToList()
            };
            return View(model);
        }

        public ActionResult Create() {
            var model = new ReceptorCreateViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(ReceptorCreateViewModel model) {
            //var model = new EmisorCreateViewModel();
            var receptor = new Receptor();

            receptor.RFC = model.RFC;

            if (!string.IsNullOrEmpty(model.Nombre))
                receptor.Nombre = model.Nombre;

            if (model.Domicilio != null) {
                if (!string.IsNullOrEmpty(model.Domicilio.Pais)) {
                    receptor.Domicilio = new Ubicacion {
                        Pais = model.Domicilio.Pais,
                        Calle = string.IsNullOrEmpty(model.Domicilio.Calle) ? null : model.Domicilio.Calle,
                        NoExterior = string.IsNullOrEmpty(model.Domicilio.NoExterior) ? null : model.Domicilio.NoExterior,
                        NoInterior = string.IsNullOrEmpty(model.Domicilio.NoInterior) ? null : model.Domicilio.NoInterior,
                        Colonia = string.IsNullOrEmpty(model.Domicilio.Colonia) ? null : model.Domicilio.Colonia,
                        Localidad = string.IsNullOrEmpty(model.Domicilio.Localidad) ? null : model.Domicilio.Localidad,
                        Municipio = string.IsNullOrEmpty(model.Domicilio.Municipio) ? null : model.Domicilio.Municipio,
                        Estado = string.IsNullOrEmpty(model.Domicilio.Estado) ? null : model.Domicilio.Estado,
                        CodigoPostal = string.IsNullOrEmpty(model.Domicilio.CodigoPostal) ? null : model.Domicilio.CodigoPostal,
                        Referencia = string.IsNullOrEmpty(model.Domicilio.Referencia) ? null : model.Domicilio.Referencia
                    };
                }
            }           

            //if (!string.IsNullOrEmpty(model.RegimenFiscal))
            //    receptor.RegimenFiscal = new List<RegimenFiscal> {
            //        new RegimenFiscal {
            //            Regimen = model.RegimenFiscal
            //        }
            //    };

            this.DBContext.Receptores.Add(receptor);
            this.DBContext.SaveChanges();

            //return RedirectToAction("Details", new { Id = receptor.PublicKey.ToString("N") }); // "Index", "Home");
            return RedirectToAction("Index");
        }

        public ActionResult Details(string id) {
            Guid publicKey;
            if (!Guid.TryParse(id, out publicKey))
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var receptor = DBContext.Receptores.Where(e => e.PublicKey == publicKey).SingleOrDefault();

            if (receptor == null)
                return HttpNotFound();

            var model = new ReceptorDetailViewModel(receptor);
            return View(model);
        }
    }
}