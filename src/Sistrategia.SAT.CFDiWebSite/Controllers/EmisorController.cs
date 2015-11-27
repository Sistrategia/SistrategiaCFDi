using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistrategia.SAT.CFDiWebSite.Models;

namespace Sistrategia.SAT.CFDiWebSite.Controllers
{
    [Authorize]
    public class EmisorController : Controller
    {
        public ActionResult Index() {
            var model = new EmisorIndexViewModel();
            return View(model);
        }

        public ActionResult Create() {
            var model = new EmisorCreateViewModel();
            return View(model);
        }
    }
}