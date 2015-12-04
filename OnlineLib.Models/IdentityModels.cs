using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

namespace OnlineLib.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class LibUser : IdentityUser<Guid,LibLogin, LibUserRole, LibClaim>
    {
        
        public string Name { get; set; }
        public string Surname { get; set; }

        public Guid AdresId { get; set; }
        public virtual Address Adress { get; set; }
        public virtual ICollection<Library> Libraries { get; set; }
        public virtual ICollection<LoanActivity> BookedBooks { get; set; }

        public LibUser()
        {
            Libraries = new HashSet<Library>();
            BookedBooks = new HashSet<LoanActivity>();
        }

        
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<LibUser, Guid> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        
    }

    public class LibUserConfiguration : EntityTypeConfiguration<LibUser>
    {
        internal LibUserConfiguration()
        {
            HasKey(x => x.Id);

            Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(x => x.Name).IsRequired().HasMaxLength(30).HasColumnName("Imie: ");
            Property(x => x.Surname).IsRequired().HasMaxLength(30).HasColumnName("Nazwisko: ");
            Property(x => x.Email).IsRequired().HasMaxLength(40).HasColumnName("Email: ");
            HasMany<Library>(x => x.Libraries).WithMany(t => t.LibUsers).Map(ul =>
            {
                ul.MapLeftKey("LibUser_Id"); 
                ul.MapRightKey("Library_Id");
                ul.ToTable("LibUserLibrary");
            });
            
            HasOptional(x => x.Adress);
            ToTable("LibUser");

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
}