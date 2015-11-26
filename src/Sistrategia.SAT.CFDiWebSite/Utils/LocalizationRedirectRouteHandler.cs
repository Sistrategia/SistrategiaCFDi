using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sistrategia.SAT.CFDiWebSite.Utils
{
    public class LocalizationRedirectRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext) {
            var routeValues = requestContext.RouteData.Values;

            var cookieLocale = requestContext.HttpContext.Request.Cookies["locale"];
            if (cookieLocale != null) {
                if (string.IsNullOrEmpty(cookieLocale.Value))
                    routeValues["culture"] = CultureInfo.CurrentUICulture.Name;
                else
                    routeValues["culture"] = cookieLocale.Value;
                return new CustomRedirectHandler(new UrlHelper(requestContext).RouteUrl(routeValues));
            }

            var uiCulture = CultureInfo.CurrentUICulture;
            //if (uiCulture.Name.StartsWith("es-"))
            //    routeValues["culture"] = "es-MX"; // intercept default es-ES for "es".
            //else
            routeValues["culture"] = uiCulture.Name;
            return new CustomRedirectHandler(new UrlHelper(requestContext).RouteUrl(routeValues));
        }
    }
}