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
    }
}