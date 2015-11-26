using System;
//using System.Data.Entity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Infrastructure.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistrategia.SAT.CFDiWebSite.Security
{
    public class SecurityUser : IdentityUser<int, SecurityUserLogin, SecurityUserRole, SecurityUserClaim>
    {
        public SecurityUser()
            : base() {
            this.PublicKey = Guid.NewGuid();
        }

        public SecurityUser(string userName)
            : this() {
            UserName = userName;
        }

        [Required]
        public Guid PublicKey { get; set; }

        [MaxLength(256)]
        public string FullName { get; set; }



        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<SecurityUser, int> manager) {            
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);            
            return userIdentity;
        }   
    }
}