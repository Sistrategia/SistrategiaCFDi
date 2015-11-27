using System.Web;
using System.Web.Mvc;

namespace Sistrategia.SAT.CFDiWebSite
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
            filters.Add(new HandleErrorAttribute());
        }
    }
}