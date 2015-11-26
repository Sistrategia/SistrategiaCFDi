namespace Sistrategia.SAT.CFDiWebSite.Migrations
{
    using Sistrategia.SAT.CFDiWebSite.Security;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Sistrategia.SAT.CFDiWebSite.Data.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Sistrategia.SAT.CFDiWebSite.Data.ApplicationDbContext";
        }

        protected override void Seed(Sistrategia.SAT.CFDiWebSite.Data.ApplicationDbContext context)
        {
             context.Roles.AddOrUpdate(
                r => r.Name,
                new SecurityRole { Id = 1, Name = "User" },
                new SecurityRole { Id = 2, Name = "Administrator" },
                new SecurityRole { Id = 3, Name = "Developer" }
             );
             context.SaveChanges();
        }
    }
}
