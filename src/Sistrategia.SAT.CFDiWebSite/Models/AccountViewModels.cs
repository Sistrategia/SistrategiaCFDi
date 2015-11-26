using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
//using Microsoft.Owin.Security;
using Sistrategia.SAT.Resources;

namespace Sistrategia.SAT.CFDiWebSite.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Password")]
        public string Password { get; set; }

        //[Display(Name = "Remember me?")]
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Account_RememberMe")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessageResourceType = typeof(LocalizedStrings), ErrorMessageResourceName = "FullNameRequired")]
        [Display(ResourceType = typeof(LocalizedStrings), Name = "FullNameField")]
        public string FullName { get; set; }

        [Required(ErrorMessageResourceType = typeof(LocalizedStrings), ErrorMessageResourceName = "EmailRequired")]
        [EmailAddress]
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100,
            MinimumLength = 6,
            ErrorMessageResourceType = typeof(LocalizedStrings),
            ErrorMessageResourceName = "Account_PasswordValidationError"
            //ErrorMessage = "The {0} must be at least {2} characters long."
            )]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(LocalizedStrings), Name = "Account_ConfirmPassword")]
        [Compare("Password",
            ErrorMessageResourceType = typeof(LocalizedStrings),
            //ErrorMessage = "The password and confirmation password do not match."
            ErrorMessageResourceName = "Account_ConfirmPasswordDoesNotMatchError"
            )]
        public string ConfirmPassword { get; set; }

        //[Display(Name = "Hometown")]
        //public string Hometown { get; set; }
    }
}