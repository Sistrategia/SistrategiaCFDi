using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistrategia.SAT.CFDiWebSite.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // GET: Home
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated) {
                return RedirectToAction("Welcome");
            }

            return View();
        }

        [AllowAnonymous]
        public ActionResult Welcome() {
            return View();
        }

        
    }
}