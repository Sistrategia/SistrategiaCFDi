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
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(ComprobanteCreateViewModel model) {
            

            //var comprobante = DBContext.Comprobantes.Where(e => e.PublicKey == publicKey).SingleOrDefault();

            //if (comprobante == null)
            //    return HttpNotFound();

            //var model = new ComprbanteDetailViewModel(comprobante);
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