using System;
using System.Collections.Generic;
//using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Sistrategia.SAT.CFDiWebSite.Security;

namespace Sistrategia.SAT.CFDiWebSite.Data
{
    public class SecurityUserStore : UserStore<SecurityUser, SecurityRole, int, SecurityUserLogin, SecurityUserRole, SecurityUserClaim>
    {
        public SecurityUserStore(ApplicationDbContext context)
            : base(context) {
        }
    }
}