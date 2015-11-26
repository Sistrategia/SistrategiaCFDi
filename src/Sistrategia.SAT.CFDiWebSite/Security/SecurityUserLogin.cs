using System;
//using System.Data.Entity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistrategia.SAT.CFDiWebSite.Security
{
    public class SecurityUserLogin : IdentityUserLogin<int>
    {
    }
}
