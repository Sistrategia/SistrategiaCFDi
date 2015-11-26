//using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistrategia.SAT.CFDiWebSite.Security
{
    public class SecurityRole : IdentityRole<int, SecurityUserRole>
    {
        public SecurityRole() { }
        public SecurityRole(string name) { Name = name; }
    }
}
