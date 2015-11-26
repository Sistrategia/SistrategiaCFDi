using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistrategia.SAT.CFDiWebSite.Controllers
{
    [Authorize]
    public class DevController : BaseController
    {
        // GET: Dev
        public ActionResult Index()
        {
            return View();
        }
    }
}