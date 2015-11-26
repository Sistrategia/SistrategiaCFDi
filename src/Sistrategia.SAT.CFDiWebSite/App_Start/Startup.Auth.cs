using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
//using Microsoft.Owin.Security.Google;
using Owin;
//using Sistrategia.SAT.Business;
using System.Configuration;
using Sistrategia.SAT.CFDiWebSite.Security;
//using Microsoft.Owin.Security.Google;

namespace Sistrategia.SAT.CFDiWebSite
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app) {            
            app.CreatePerOwinContext<SecurityUserManager>(SecurityUserManager.Create);
            app.CreatePerOwinContext<SecuritySignInManager>(SecuritySignInManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<SecurityUserManager, SecurityUser, int>(
                        validateInterval: TimeSpan.FromMinutes(30),                      
                        regenerateIdentityCallback: (manager, user) => user.GenerateUserIdentityAsync(manager),
                        getUserIdCallback: (id) => id.GetUserId<int>()
                    )
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5)); // .FromMinutes(1));

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions() {
            //    ClientId = ConfigurationManager.AppSettings["GoogleClientId"],
            //    ClientSecret = ConfigurationManager.AppSettings["GoogleClientSecret"]
            //});
        }
    }
}