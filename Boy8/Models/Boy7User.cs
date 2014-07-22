using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Boy8.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class Boy7User : IdentityUser
    {
        public string HomeTown { get; set; }

        public System.DateTime? BirthDate { get; set; }
               
        public bool HasBabyLinked { get; set; }

        //[ForeignKey("Baby")]
        //public Guid? BabyID { get; set; }

        public virtual ICollection<Baby> Babies { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<Boy7User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

}