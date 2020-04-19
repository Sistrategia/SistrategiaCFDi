using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Sistrategia.SAT.CFDiWebSite.Data;

[assembly: OwinStartup(typeof(Sistrategia.SAT.CFDiWebSite.Startup))]

namespace Sistrategia.SAT.CFDiWebSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app) {

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            //AreaRegistration.RegisterAllAreas();
            //GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            app.CreatePerOwinContext(ApplicationDbContext.Create);

            ConfigureAuth(app);
            
        }
    }
}
