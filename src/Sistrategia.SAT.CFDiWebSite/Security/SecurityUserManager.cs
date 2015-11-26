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
using Sistrategia.SAT.CFDiWebSite.Messaging;
using Sistrategia.SAT.CFDiWebSite.Data;

namespace Sistrategia.SAT.CFDiWebSite.Security
{
    public class SecurityUserManager : UserManager<SecurityUser, int>
    {
        public SecurityUserManager(IUserStore<SecurityUser, int> store)
            : base(store) {
        }

        public static SecurityUserManager Create(IdentityFactoryOptions<SecurityUserManager> options, IOwinContext context) {
            var manager = new SecurityUserManager(new SecurityUserStore(context.Get<ApplicationDbContext>()));
            manager.UserValidator = new UserValidator<SecurityUser, int>(manager) {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            manager.PasswordValidator = new PasswordValidator {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false, //true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;
            //manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<SecurityUser, int> {
            //    Subject = "Security Code",
            //    BodyFormat = "Your security code is {0}"
            //});

            manager.EmailService = new EmailService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null) {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<SecurityUser, int>(dataProtectionProvider.Create("ASP.NET Identity")); // {
                //  TokenLifespan = TimeSpan.FromHours(3)
                //};
            }

            return manager;
        }
    }
}