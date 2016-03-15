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

            if (requestContext.HttpContext.Request.Headers["Accept-Language"].StartsWith("es,")) {
                routeValues["culture"] = "es-MX";
            }
            else {                
                routeValues["culture"] = CultureInfo.CurrentUICulture.Name;
            }            
            
            return new CustomRedirectHandler(new UrlHelper(requestContext).RouteUrl(routeValues));
        }
    }
}