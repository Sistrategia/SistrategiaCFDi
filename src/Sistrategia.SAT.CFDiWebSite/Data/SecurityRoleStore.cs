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
    public class SecurityRoleStore : RoleStore<SecurityRole, int, SecurityUserRole>
    {
        public SecurityRoleStore(ApplicationDbContext context)
            : base(context) {
        }
    }
}
