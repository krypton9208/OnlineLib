using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace OnlineLib.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class LibUser : IdentityUser<Guid,LibLogin, LibUserRole, LibClaim>
    {
        
        public string Name { get; set; }
        public string Surname { get; set; }
        public virtual Address Adress { get; set; }
        public virtual ICollection<Library> Library { get; set; }
        public virtual ICollection<Book> BookedBooks { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<LibUser, Guid> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class LibClaim : IdentityUserClaim<Guid>
    {
    }

    public class LibUserRole : IdentityUserRole<Guid>
    {
    }

    public class LibLogin : IdentityUserLogin<Guid>
    {
    }
    public class LibRole : IdentityRole<Guid, LibUserRole>
    {
    }
    public class LibUserMap : EntityTypeConfiguration<LibUser>
    {
        public LibUserMap()
        {
        }

        //public static OnlineLibContext Create()
        //{
        //    return new OnlineLibContext();
        //}
    }
}