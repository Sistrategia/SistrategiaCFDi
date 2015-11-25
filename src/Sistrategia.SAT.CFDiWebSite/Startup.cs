using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

namespace Sistrategia.SAT.CFDiWebSite
{
    using AppFunc = Func<IDictionary<string, object>, Task>;

    public class Startup
    {
        public AppFunc Configuration() {
            return async env => {
                var writer = new StreamWriter((Stream)env["owin.ResponseBody"]);
                await writer.WriteAsync("SistrategiaCFDi");
                await writer.FlushAsync();
            };
        }
    }
}
