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
        public Configuration() {
            AutomaticMigrationsEnabled = false;
            ContextKey = "Sistrategia.SAT.CFDiWebSite.Data.ApplicationDbContext";
        }

        public void ReSeed(Sistrategia.SAT.CFDiWebSite.Data.ApplicationDbContext context) {
            this.Seed(context);
        }

        protected override void Seed(Sistrategia.SAT.CFDiWebSite.Data.ApplicationDbContext context) {
            context.Roles.AddOrUpdate(
               r => r.Name,
               new SecurityRole { Id = 1, Name = "User" },
               new SecurityRole { Id = 2, Name = "Administrator" },
               new SecurityRole { Id = 3, Name = "Backstage" },
               new SecurityRole { Id = 4, Name = "Developer" }
            );
            context.SaveChanges();

            context.TiposTipoDeComprobante.AddOrUpdate(
                t => t.TipoTipoDeComprobanteValue,
                new TipoTipoDeComprobante { TipoTipoDeComprobanteId = 1, TipoTipoDeComprobanteValue = "ingreso" },
                new TipoTipoDeComprobante { TipoTipoDeComprobanteId = 1, TipoTipoDeComprobanteValue = "egreso" },
                new TipoTipoDeComprobante { TipoTipoDeComprobanteId = 1, TipoTipoDeComprobanteValue = "traslado" }
            );
            context.SaveChanges();

            context.TiposImpuestoRetencion.AddOrUpdate(
                t => t.TipoImpuestoRetencionValue,
                new TipoImpuestoRetencion { TipoImpuestoRetencionId = 1, TipoImpuestoRetencionValue = "ISR" },
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
                //Migration:SATSchema
                //new TipoMetodoDePago { TipoMetodoDePagoId = 1, TipoMetodoDePagoValue = "EFECTIVO" },
                //new TipoMetodoDePago { TipoMetodoDePagoId = 2, TipoMetodoDePagoValue = "CHEQUE" },
                //new TipoMetodoDePago { TipoMetodoDePagoId = 3, TipoMetodoDePagoValue = "TRANSFERENCIA INTERBANCARIA" },
                //new TipoMetodoDePago { TipoMetodoDePagoId = 4, TipoMetodoDePagoValue = "NO IDENTIFICADO" },
                //new TipoMetodoDePago { TipoMetodoDePagoId = 5, TipoMetodoDePagoValue = "TARJETA DE CRÉDITO" },
                //new TipoMetodoDePago { TipoMetodoDePagoId = 6, TipoMetodoDePagoValue = "TARJETA DE DÉBITO" }

                //Migration:MetodoDePago2016
                 new TipoMetodoDePago { TipoMetodoDePagoId = 1, TipoMetodoDePagoValue = "EFECTIVO", TipoMetodoDePagoCode = null, TipoMetodoDePagoDescription = "EFECTIVO", Status = "I" },
                 new TipoMetodoDePago { TipoMetodoDePagoId = 2, TipoMetodoDePagoValue = "CHEQUE", TipoMetodoDePagoCode = null, TipoMetodoDePagoDescription = "CHEQUE", Status = "I" },
                 new TipoMetodoDePago { TipoMetodoDePagoId = 3, TipoMetodoDePagoValue = "TRANSFERENCIA INTERBANCARIA", TipoMetodoDePagoCode = null, TipoMetodoDePagoDescription = "TRANSFERENCIA INTERBANCARIA", Status = "I" },
                 new TipoMetodoDePago { TipoMetodoDePagoId = 4, TipoMetodoDePagoValue = "NO IDENTIFICADO", TipoMetodoDePagoCode = null, TipoMetodoDePagoDescription = "NO IDENTIFICADO", Status = "I" },
                 new TipoMetodoDePago { TipoMetodoDePagoId = 5, TipoMetodoDePagoValue = "TARJETA DE CRÉDITO", TipoMetodoDePagoCode = null, TipoMetodoDePagoDescription = "TARJETA DE CRÉDITO", Status = "I" },
                 new TipoMetodoDePago { TipoMetodoDePagoId = 6, TipoMetodoDePagoValue = "TARJETA DE DÉBITO", TipoMetodoDePagoCode = null, TipoMetodoDePagoDescription = "TARJETA DE DÉBITO", Status = "I" },

                 new TipoMetodoDePago { TipoMetodoDePagoId = 7, TipoMetodoDePagoValue = "DEPOSITO BANCARIO", TipoMetodoDePagoCode = null, TipoMetodoDePagoDescription = "DEPOSITO BANCARIO", Status = "I" },
                 new TipoMetodoDePago { TipoMetodoDePagoId = 8, TipoMetodoDePagoValue = "TRANSFERENCIA", TipoMetodoDePagoCode = null, TipoMetodoDePagoDescription = "TRANSFERENCIA", Status = "I" },
                 new TipoMetodoDePago { TipoMetodoDePagoId = 9, TipoMetodoDePagoValue = "No Identificado", TipoMetodoDePagoCode = null, TipoMetodoDePagoDescription = "No Identificado", Status = "I" },

                 new TipoMetodoDePago { TipoMetodoDePagoId = 10, TipoMetodoDePagoValue = "01", TipoMetodoDePagoCode = "01", TipoMetodoDePagoDescription = "EFECTIVO", Status = "A" },
                 new TipoMetodoDePago { TipoMetodoDePagoId = 11, TipoMetodoDePagoValue = "02", TipoMetodoDePagoCode = "02", TipoMetodoDePagoDescription = "CHEQUE NOMINATIVO", Status = "A" },
                 new TipoMetodoDePago { TipoMetodoDePagoId = 12, TipoMetodoDePagoValue = "03", TipoMetodoDePagoCode = "03", TipoMetodoDePagoDescription = "TRANSFERENCIA ELECTRÓNICA DE FONDOS", Status = "A" },
                 new TipoMetodoDePago { TipoMetodoDePagoId = 13, TipoMetodoDePagoValue = "04", TipoMetodoDePagoCode = "04", TipoMetodoDePagoDescription = "TARJETA DE CRÉDITO", Status = "A" },
                 new TipoMetodoDePago { TipoMetodoDePagoId = 14, TipoMetodoDePagoValue = "05", TipoMetodoDePagoCode = "05", TipoMetodoDePagoDescription = "MONEDERO ELECTRÓNICO", Status = "A" },
                 new TipoMetodoDePago { TipoMetodoDePagoId = 15, TipoMetodoDePagoValue = "06", TipoMetodoDePagoCode = "06", TipoMetodoDePagoDescription = "DINERO ELECTRÓNICO", Status = "A" },
                 new TipoMetodoDePago { TipoMetodoDePagoId = 16, TipoMetodoDePagoValue = "08", TipoMetodoDePagoCode = "08", TipoMetodoDePagoDescription = "VALES DE DESPENSA", Status = "A" },
                 new TipoMetodoDePago { TipoMetodoDePagoId = 17, TipoMetodoDePagoValue = "28", TipoMetodoDePagoCode = "28", TipoMetodoDePagoDescription = "TARJETA DE DÉBITO", Status = "A" },
                 new TipoMetodoDePago { TipoMetodoDePagoId = 18, TipoMetodoDePagoValue = "29", TipoMetodoDePagoCode = "29", TipoMetodoDePagoDescription = "TARJETA DE SERVICIO", Status = "A" },
                 new TipoMetodoDePago { TipoMetodoDePagoId = 19, TipoMetodoDePagoValue = "99", TipoMetodoDePagoCode = "99", TipoMetodoDePagoDescription = "OTROS", Status = "A" }
           );
            context.SaveChanges();

            context.TiposMoneda.AddOrUpdate(
                t => t.TipoMonedaValue,
                 new TipoMoneda { TipoMonedaId = 1, TipoMonedaValue = "MXN" },
                 new TipoMoneda { TipoMonedaId = 2, TipoMonedaValue = "USD" }
                );
            context.SaveChanges();

            context.Bancos.AddOrUpdate(
                b => b.Clave,
                new Banco { BancoId = 1, PublicKey = new Guid("82E2C8AC-06AE-4F61-AFA6-364C731EF65D"), Clave = "002", NombreCorto = "BANAMEX", RazonSocial = "Banco Nacional de México, S.A., Institución de Banca Múltiple, Grupo Financiero Banamex", Status = "A" },
                new Banco { BancoId = 2, PublicKey = new Guid("777AA59F-B61D-47C8-999F-C594670902DE"), Clave = "006", NombreCorto = "BANCOMEXT", RazonSocial = "Banco Nacional de Comercio Exterior, Sociedad Nacional de Crédito, Institución de Banca de Desarrollo", Status = "A" }
            );
            context.SaveChanges();


            context.TiposFormaDePago.AddOrUpdate(
                t => t.TipoFormaDePagoValue,
                new TipoFormaDePago { TipoFormaDePagoId = 1, TipoFormaDePagoValue = "PAGO EN UNA SOLA EXHIBICION" }
            );
            context.SaveChanges();

            context.ViewTemplates.AddOrUpdate(
                v => v.CodeName,
                new ViewTemplate { ViewTemplateId = 1, CodeName = "ddm1", DisplayName = "ddm1", Description = "ddm1" },
                new ViewTemplate { ViewTemplateId = 2, CodeName = "ddm2", DisplayName = "ddm2", Description = "ddm2" },
                new ViewTemplate { ViewTemplateId = 3, CodeName = "cemer", DisplayName = "cemer", Description = "cemer" }
            );
            context.SaveChanges();
        }
    }
}
