namespace Sistrategia.SAT.CFDiWebSite.Migrations
{
    using Sistrategia.SAT.CFDiWebSite.CFDI;
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
                new SecurityRole { Id = 3, Name = "Backstage" },
                new SecurityRole { Id = 4, Name = "Developer" }
             );
             context.SaveChanges();

             context.ViewTemplates.AddOrUpdate(
                 v => v.CodeName,
                 new ViewTemplate { ViewTemplateId = 1, CodeName = "ddm1", DisplayName = "ddm1", Description = "ddm1"},
                 new ViewTemplate { ViewTemplateId = 2, CodeName = "ddm2", DisplayName = "ddm2", Description = "ddm2"}
             );
             context.SaveChanges();
        }
    }
}
