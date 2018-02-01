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
using Sistrategia.SAT.CFDiWebSite.Models;
using Sistrategia.SAT.Resources;

namespace Sistrategia.SAT.CFDiWebSite.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        #region Constructor
        public AccountController() {
        }

        public AccountController(SecurityUserManager userManager, SecuritySignInManager signInManager, ApplicationDbContext applicationDBContext)
            : base(userManager, signInManager, applicationDBContext) {

        }
        #endregion

        #region Index (MyAccount)
        public async Task<ActionResult> Index(AccountIndexMessageId? message)
        {
            ViewBag.StatusMessage =
                message == AccountIndexMessageId.ChangePasswordSuccess ? LocalizedStrings.Account_YourPasswordHasBeenChanged
                : message == AccountIndexMessageId.SetPasswordSuccess ? LocalizedStrings.Account_YourPasswordHasBeenSet
                : message == AccountIndexMessageId.SetTwoFactorSuccess ? LocalizedStrings.Account_YourTwoFactorAuthenticationHasBeenSet
                : message == AccountIndexMessageId.Error ? LocalizedStrings.Account_MessageError
                : message == AccountIndexMessageId.AddPhoneSuccess ? LocalizedStrings.Account_MessageAddPhoneSuccess
                : message == AccountIndexMessageId.RemovePhoneSuccess ? LocalizedStrings.Account_MessageRemovePhoneSuccess
                : "";

            var userId = User.Identity.GetUserId<int>();

            if (userId != default(int)) {
                var user = await UserManager.FindByIdAsync(userId);
                var model = new AccountIndexViewModel {
                    UserName = user.UserName,
                    FullName = user.FullName,
                    HasPassword = HasPassword(),
                    PhoneNumber = user.PhoneNumber,
                    TwoFactor = user.TwoFactorEnabled,
                    Logins = await UserManager.GetLoginsAsync(userId),
                    BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(User.Identity.GetUserId()), // string always?                    
                };
                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Login and Logoff
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl) {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl) {
            if (!ModelState.IsValid) {
                return View(model);
            }

            if (!model.Email.EndsWith("@sistrategia.com"))
                return View("AccountManualValidation");

            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: true);
            switch (result) {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // POST: /Account/LogOff
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult LogOff() {
        //    AuthenticationManager.SignOut();
        //    return RedirectToAction("Index", "Home");
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LogOff() {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Register and Email Confirmation
        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register() {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public /*async Task<ActionResult>*/ ActionResult Register(RegisterViewModel model) {
            //if (ModelState.IsValid) {
            //    var user = new SecurityUser { UserName = model.Email, Email = model.Email, FullName = model.FullName }; //, Hometown = model.Hometown };
            //    var result = await UserManager.CreateAsync(user, model.Password);
            //    if (result.Succeeded) {
            //        // await UserManager.AddToRoleAsync(user.Id, "User");
            //        // await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);                    

            //        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
            //        // Send an email with this link
            //        string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            //        var callbackUrl = Url.Action("ConfirmEmail", "Account",
            //            new { invitation = user.PublicKey.ToString("N"), code = code }
            //            , protocol: Request.Url.Scheme);

            //        await UserManager.SendEmailAsync(user.Id,
            //            LocalizedStrings.Account_ConfirmYourAccount + " " + DateTime.Now.ToString("yyyyMMddHHmmssfff"),
            //            string.Format(LocalizedStrings.Account_ConfirmYourAccountBody, callbackUrl));

            //        //return RedirectToAction("Index", "Home");
            //        return View("DisplayEmail");
            //    }
            //    AddErrors(result);
            //}

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult DisplayEmail() {
            return View();
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string invitation, string code) {            
            if (invitation == null || code == null) {
                return View("Error");
            }
            
            var invId = Guid.Parse(invitation);
            var user = UserManager.Users.SingleOrDefault(u => u.PublicKey == invId);

            if (user == null) {
                return View("Error");
            }

            var result = await UserManager.ConfirmEmailAsync(user.Id, code);

            if (result.Succeeded) {
                //string containerId = user.PublicKey.ToString("N");
                //CloudStorageMananger manager = new CloudStorageMananger(DBContext);
                //CloudStorageContainer container = manager.CreateContainer("Azure", user.PublicKey, user.UserName, string.Format("{0}'s default storage container", user.UserName));
                //user.DefaultContainer = container;
                UserManager.Update(user);
            }

            return View(result.Succeeded ? "ConfirmEmail" : "Error");
            //if (result.Succeeded)
            //    return View("ConfirmEmail");
            //AddErrors(result);
            //return View("Error");            
        }

        #endregion

        #region Forgot and Reset Password

        [AllowAnonymous]
        public ActionResult ForgotPassword() {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model) {
            if (ModelState.IsValid) {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id))) {
                    return View("ForgotPasswordConfirmation");
                }

                string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { forgot = user.PublicKey, code = code }, protocol: Request.Url.Scheme);
                await UserManager.SendEmailAsync(user.Id, LocalizedStrings.Account_ResetPassword,
                    string.Format(LocalizedStrings.Account_PleaseResetYourPasswordByClickingHere, callbackUrl));
                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation() {
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword(string code) {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model) {
            if (!ModelState.IsValid) {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null) {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded) {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation() {
            return View();
        }

        #endregion

        #region Change Password
        
        public ActionResult ChangePassword() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model) {
            if (!ModelState.IsValid) {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(this.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded) {
                var user = await UserManager.FindByIdAsync(this.GetUserId());
                if (user != null) {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = AccountIndexMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        #endregion

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager {
            get {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result) {
            foreach (var error in result.Errors) {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl) {
            if (Url.IsLocalUrl(returnUrl)) {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null) {
            }

            public ChallengeResult(string provider, string redirectUri, string userId) {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context) {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null) {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }



        private bool HasPassword() {
            var user = UserManager.FindById(this.GetUserId());
            if (user != null) {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber() {
            var user = UserManager.FindById(this.GetUserId());
            if (user != null) {
                return user.PhoneNumber != null;
            }
            return false;
        }

        #endregion
    }
}