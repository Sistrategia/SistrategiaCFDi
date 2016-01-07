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

             context.TiposImpuestoRetencion.AddOrUpdate(
                 t => t.TipoImpuestoRetencionValue,
                 new TipoImpuestoRetencion { TipoImpuestoRetencionId = 1, TipoImpuestoRetencionValue = "ISR"},
                 new TipoImpuestoRetencion { TipoImpuestoRetencionId = 2, TipoImpuestoRetencionValue = "IVA" }
                 );
             context.SaveChanges();

             context.TiposImpuestoTraslado.AddOrUpdate(
                  t => t.TipoImpuestoTrasladoValue,
                  new TipoImpuestoTraslado { TipoImpuestoTrasladoId = 1, TipoImpuestoTrasladoValue = "IVA" },
                  new TipoImpuestoTraslado { TipoImpuestoTrasladoId = 2, TipoImpuestoTrasladoValue = "IEPS" }
                  );
             context.SaveChanges();

             context.TiposMetodoDePago.AddOrUpdate(
                 t => t.TipoMetodoDePagoValue,
                 new TipoMetodoDePago { TipoMetodoDePagoId = 1, TipoMetodoDePagoValue = "EFECTIVO" },
                 new TipoMetodoDePago { TipoMetodoDePagoId = 2, TipoMetodoDePagoValue = "CHEQUE" },
                 new TipoMetodoDePago { TipoMetodoDePagoId = 3, TipoMetodoDePagoValue = "TRANSFERENCIA INTERBANCARIA" },
                 new TipoMetodoDePago { TipoMetodoDePagoId = 4, TipoMetodoDePagoValue = "NO IDENTIFICADO" },
                 new TipoMetodoDePago { TipoMetodoDePagoId = 5, TipoMetodoDePagoValue = "TARJETA DE CRÉDITO" },
                 new TipoMetodoDePago { TipoMetodoDePagoId = 6, TipoMetodoDePagoValue = "TARJETA DE DÉBITO" }
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
