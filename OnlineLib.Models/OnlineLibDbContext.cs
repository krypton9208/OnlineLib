using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
//using System.EnterpriseServices;
using System.Linq;
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
       // public DbSet<Activity> Activity { get; set; }
       // public DbSet<Settings> Settings { get; set; }
        DbEntityEntry Entry(object entity);

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Configurations.Add(new Book());
            //modelBuilder.Configurations.Add(new Library());
            //modelBuilder.Configurations.Add(new Address());
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
            modelBuilder.Entity<Book>()
                .Property(r => r.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<Library>()
                .Property(r => r.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);


            base.OnModelCreating(modelBuilder);
        }

        public static OnlineLibDbContext Create()
        {
            return new OnlineLibDbContext();
        }

    }
}
