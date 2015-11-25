using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

[assembly: OwinStartup(typeof(Sistrategia.SAT.CFDiWebSite.Startup))]

namespace Sistrategia.SAT.CFDiWebSite
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class Startup
    {
        public void Configuration(IAppBuilder app) {

            //AreaRegistration.RegisterAllAreas();
            //GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            //return async env => {
            //    var writer = new StreamWriter((Stream)env["owin.ResponseBody"]);
            //    await writer.WriteAsync("SistrategiaCFDi");
            //    await writer.FlushAsync();
            //};
        }
    }
}
