using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Sistrategia.SAT.CFDiWebSite.Data;
using Sistrategia.SAT.CFDiWebSite.Security;

//using Sistrategia.SAT.Business;
//using Sistrategia.SAT.CFDiWebSite.Models;
//using Sistrategia.SAT.CFDiWebSite.Resources;

namespace Sistrategia.SAT.CFDiWebSite.Controllers
{
    public class BaseController : Controller
    {
        private SecuritySignInManager signInManager;
        private SecurityUserManager userManager;
        private ApplicationDbContext applicationDBContext;

        public BaseController() {

        }

        public BaseController(SecurityUserManager userManager, SecuritySignInManager signInManager, ApplicationDbContext applicationDBContext) {
            UserManager = userManager;
            SignInManager = signInManager;
            DBContext = applicationDBContext;
        }

        public ApplicationDbContext DBContext {
            get { return applicationDBContext ?? HttpContext.GetOwinContext().Get<ApplicationDbContext>(); }
            private set { applicationDBContext = value; }
        }

        public SecuritySignInManager SignInManager {
            get { return signInManager ?? HttpContext.GetOwinContext().Get<SecuritySignInManager>(); }
            private set { signInManager = value; }
        }

        public SecurityUserManager UserManager {
            get { return userManager ?? HttpContext.GetOwinContext().GetUserManager<SecurityUserManager>(); }
            private set { userManager = value; }
        }

        public int GetUserId() {
            return User.Identity.GetUserId<int>();
            //return int.Parse(User.Identity.GetUserId());
        }

        public SecurityUser CurrentSecurityUser {
            get {
                var userId = this.GetUserId(); // int.Parse( User.Identity.GetUserId() );
                return UserManager.FindById(userId);
            }
        }


        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (userManager != null) {
                    userManager.Dispose();
                    userManager = null;
                }

                if (signInManager != null) {
                    signInManager.Dispose();
                    signInManager = null;
                }

                if (applicationDBContext != null) {
                    applicationDBContext.Dispose();
                    applicationDBContext = null;
                }
            }
            base.Dispose(disposing);
        }
    }
}