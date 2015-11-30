using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
//using System.EnterpriseServices;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;
using OnlineLib.Models;


namespace OnlineLib.Models
{
    public class OnlineLibDbContext : IdentityDbContext<LibUser, LibRole, Guid, LibLogin, LibUserRole, LibClaim>
    {
        public DbSet<Book> Book { get; set; }
        public DbSet<Library> Library { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<LibraryLibUser> LibraryLibUser { get; set; }
        //public DbSet<LibUserRole> LibUserRole { get; set; }
        //public DbSet<LibUser> LibUser { get; set; }
       // public DbSet<Activity> Activity { get; set; }
       // public DbSet<Settings> Settings { get; set; }
        DbEntityEntry Entry(object entity)
        {
            throw new NotImplementedException();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BookConfiguration());
            modelBuilder.Configurations.Add(new LibraryConfiguration());
            modelBuilder.Configurations.Add(new LibUserConfiguration());
            modelBuilder.Configurations.Add(new LibraryLibUserConfiguration());
            //modelBuilder.Configurations.Add(new LibUserMap());
          //  modelBuilder.Configurations.Add(new WtUserMap());
          //  modelBuilder.Configurations.Add(new SettingseMap());

            modelBuilder.Entity<LibUser>()
                .Property(r => r.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<LibClaim>()
                .Property(r => r.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<LibRole>()
                .Property(r => r.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            //modelBuilder.Entity<Library>().HasRequired(x => x.Address).WithMany().HasForeignKey(t => t.Address);
            //modelBuilder.Entity<Library>().HasRequired(x => x.Owner).WithMany().HasForeignKey(t => t.Owner);

            base.OnModelCreating(modelBuilder);
        }

        public static OnlineLibDbContext Create()
        {
            return new OnlineLibDbContext();
        }

    }
}
