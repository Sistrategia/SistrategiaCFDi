// Thanks to: Soe Moe Tun Lwin
// http://www.jittuu.com/2014/3/17/AspNet-localization-routing/
using System;
using System.Diagnostics.CodeAnalysis;
using System.Web;

namespace Sistrategia.SAT.CFDiWebSite.Utils
{
    class CustomRedirectHandler : IHttpHandler
    {
        private string _newUrl;

        [SuppressMessage(category: "Microsoft.Design", checkId: "CA1054:UriParametersShouldNotBeStrings",
            Justification = "We just use string since HttpResponse.Redirect only accept as string parameter.")]
        public CustomRedirectHandler(string newUrl) {
            this._newUrl = newUrl;
        }

        public bool IsReusable {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context) {
            context.Response.Redirect(this._newUrl);
        }
    }
}