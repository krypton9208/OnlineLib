namespace OnlineLib.Models.Migrations
{
    using System.Data.Entity.Migrations;

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
               new LibRole() { Name = "Admin" },
               new LibRole() { Name = "LibOwners" },
               new LibRole() { Name = "Main_Workers" },
               new LibRole() { Name = "Workers" },
               new LibRole() { Name = "Readers"}
               );
        }

    }
}
