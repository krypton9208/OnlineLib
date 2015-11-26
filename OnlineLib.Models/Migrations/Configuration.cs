using Microsoft.Owin.Security.DataProtection;

namespace OnlineLib.Models.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<OnlineLib.Models.OnlineLibDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(OnlineLib.Models.OnlineLibDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.Roles.AddOrUpdate(
               p => p.Name,
               new LibRole() { Name = "Admin", Id = Guid.NewGuid() },
               new LibRole() { Name = "LibOwners", Id = Guid.NewGuid() },
               new LibRole() { Name = "Workers", Id = Guid.NewGuid() },
               new LibRole() { Name = "Readers", Id = Guid.NewGuid() }
               );
        }
    }
}
