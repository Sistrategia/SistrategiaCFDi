using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sistrategia.SAT.CFDiWebSite.Controllers
{
    public class LocaleController : Controller
    {
        // http://www.hanselman.com/blog/GlobalizationInternationalizationAndLocalizationInASPNETMVC3JavaScriptAndJQueryPart1.aspx
        public ActionResult CurrentCulture() {
            return Json(System.Threading.Thread.CurrentThread.CurrentUICulture.ToString(), JsonRequestBehavior.AllowGet);
        }
        //<script>
        //    $(document).ready(function () {
        //        //Ask ASP.NET what culture we prefer
        //        $.getJSON('/locale/currentculture', function (data) {
        //            //Tell jQuery to figure it out also on the client side.
        //            $.global.preferCulture(data);
        //        });
        //    });
        //</script>

        public ActionResult ChangeLang(string lang, string returnUrl) {
            var langCookie = new HttpCookie("locale", lang) { HttpOnly = true };
            Response.AppendCookie(langCookie);
            return Redirect(HttpUtility.UrlDecode(returnUrl));
        }
    }
}