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
//using Sistrategia.Drive.WebSite.Models;

namespace Sistrategia.SAT.CFDiWebSite.Security
{
    public class SecuritySignInManager : SignInManager<SecurityUser, int>
    {
        public SecuritySignInManager(SecurityUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager) {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(SecurityUser user) {
            //return user.GenerateUserIdentityAsync((SecurityUserManager)UserManager);
            return user.GenerateUserIdentityAsync((SecurityUserManager)UserManager);
        }

        public static SecuritySignInManager Create(IdentityFactoryOptions<SecuritySignInManager> options, IOwinContext context) {
            return new SecuritySignInManager(context.GetUserManager<SecurityUserManager>(), context.Authentication);
        }
    }
}
