using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Sistrategia.SAT.CFDiWebSite.Utils;

namespace Sistrategia.SAT.CFDiWebSite
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);

            routes.MapLocalizeRoute(
                name: "Default",
                url: "{culture}/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                constraints: new { culture = "[a-zA-Z]{2}-[a-zA-Z]{2}" });

            routes.MapRouteToLocalizeRedirect("RedirectToLocalize",
                        url: "{controller}/{action}/{id}",
                        defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional });
        }
    }
}