using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistrategia.SAT.CFDiWebSite.Models;

namespace Sistrategia.SAT.CFDiWebSite.Controllers
{
    public class CertificadoController : BaseController
    {
        // GET: Certificado
        public ActionResult Index()
        {
            var model = new CertificadoIndexViewModel {
                Certificados = this.DBContext.Certificados.ToList()
            };
            return View(model);
        }

        public ActionResult Create() {
            var model = new CertificadoCreateViewModel();
            return View(model);
        }
    }
}