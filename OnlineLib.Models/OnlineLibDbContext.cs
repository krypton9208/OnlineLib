﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
//using System.EnterpriseServices;
using Microsoft.AspNet.Identity.EntityFramework;


namespace OnlineLib.Models
{
    public class OnlineLibDbContext : IdentityDbContext<LibUser, LibRole, Guid, LibLogin, LibUserRole, LibClaim>
    {
        public DbSet<Book> Book { get; set; }
        public DbSet<Library> Library { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<LoanActivity> LoanActivitie { get; set; }
        public OnlineLibDbContext()
        : base("DefaultConnection"){}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BookConfiguration());
            modelBuilder.Configurations.Add(new LibraryConfiguration());
            modelBuilder.Configurations.Add(new LibUserConfiguration());
            modelBuilder.Configurations.Add(new LoanActivityConfiguration());
          
            modelBuilder.Entity<LibClaim>()
                .Property(r => r.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<LibRole>()
                .Property(r => r.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            modelBuilder.Entity<LibUserRole>()
                .HasOptional(x => x.WorkPlace)
                .WithMany(x => x.Workers)
                .Map(d => d.MapKey("Library"));
            base.OnModelCreating(modelBuilder);
        }

        public static OnlineLibDbContext Create()
        {
            return new OnlineLibDbContext();
        }
    }
}
