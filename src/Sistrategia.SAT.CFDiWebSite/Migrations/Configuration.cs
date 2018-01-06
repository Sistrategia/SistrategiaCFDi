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
                new TipoTipoDeComprobante { TipoTipoDeComprobanteId = 1, TipoTipoDeComprobanteValue = "traslado" },
                new TipoTipoDeComprobante { TipoTipoDeComprobanteId = 1, TipoTipoDeComprobanteValue = "I" },
                new TipoTipoDeComprobante { TipoTipoDeComprobanteId = 1, TipoTipoDeComprobanteValue = "E" },
                new TipoTipoDeComprobante { TipoTipoDeComprobanteId = 1, TipoTipoDeComprobanteValue = "T" },
                new TipoTipoDeComprobante { TipoTipoDeComprobanteId = 1, TipoTipoDeComprobanteValue = "P" }
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

           // context.TiposMetodoDePago.AddOrUpdate(
           //     t => t.TipoMetodoDePagoValue,
           //     //Migration:SATSchema
           //     //new TipoMetodoDePago { TipoMetodoDePagoId = 1, TipoMetodoDePagoValue = "EFECTIVO" },
           //     //new TipoMetodoDePago { TipoMetodoDePagoId = 2, TipoMetodoDePagoValue = "CHEQUE" },
           //     //new TipoMetodoDePago { TipoMetodoDePagoId = 3, TipoMetodoDePagoValue = "TRANSFERENCIA INTERBANCARIA" },
           //     //new TipoMetodoDePago { TipoMetodoDePagoId = 4, TipoMetodoDePagoValue = "NO IDENTIFICADO" },
           //     //new TipoMetodoDePago { TipoMetodoDePagoId = 5, TipoMetodoDePagoValue = "TARJETA DE CRÉDITO" },
           //     //new TipoMetodoDePago { TipoMetodoDePagoId = 6, TipoMetodoDePagoValue = "TARJETA DE DÉBITO" }

           //     //Migration:MetodoDePago2016
           //      new TipoMetodoDePago { TipoMetodoDePagoId = 1, TipoMetodoDePagoValue = "EFECTIVO", TipoMetodoDePagoCode = null, TipoMetodoDePagoDescription = "EFECTIVO", Status = "I" },
           //      new TipoMetodoDePago { TipoMetodoDePagoId = 2, TipoMetodoDePagoValue = "CHEQUE", TipoMetodoDePagoCode = null, TipoMetodoDePagoDescription = "CHEQUE", Status = "I" },
           //      new TipoMetodoDePago { TipoMetodoDePagoId = 3, TipoMetodoDePagoValue = "TRANSFERENCIA INTERBANCARIA", TipoMetodoDePagoCode = null, TipoMetodoDePagoDescription = "TRANSFERENCIA INTERBANCARIA", Status = "I" },
           //      new TipoMetodoDePago { TipoMetodoDePagoId = 4, TipoMetodoDePagoValue = "NO IDENTIFICADO", TipoMetodoDePagoCode = null, TipoMetodoDePagoDescription = "NO IDENTIFICADO", Status = "I" },
           //      new TipoMetodoDePago { TipoMetodoDePagoId = 5, TipoMetodoDePagoValue = "TARJETA DE CRÉDITO", TipoMetodoDePagoCode = null, TipoMetodoDePagoDescription = "TARJETA DE CRÉDITO", Status = "I" },
           //      new TipoMetodoDePago { TipoMetodoDePagoId = 6, TipoMetodoDePagoValue = "TARJETA DE DÉBITO", TipoMetodoDePagoCode = null, TipoMetodoDePagoDescription = "TARJETA DE DÉBITO", Status = "I" },

           //      new TipoMetodoDePago { TipoMetodoDePagoId = 7, TipoMetodoDePagoValue = "DEPOSITO BANCARIO", TipoMetodoDePagoCode = null, TipoMetodoDePagoDescription = "DEPOSITO BANCARIO", Status = "I" },
           //      new TipoMetodoDePago { TipoMetodoDePagoId = 8, TipoMetodoDePagoValue = "TRANSFERENCIA", TipoMetodoDePagoCode = null, TipoMetodoDePagoDescription = "TRANSFERENCIA", Status = "I" },
           //      new TipoMetodoDePago { TipoMetodoDePagoId = 9, TipoMetodoDePagoValue = "No Identificado", TipoMetodoDePagoCode = null, TipoMetodoDePagoDescription = "No Identificado", Status = "I" },

           //      new TipoMetodoDePago { TipoMetodoDePagoId = 10, TipoMetodoDePagoValue = "01", TipoMetodoDePagoCode = "01", TipoMetodoDePagoDescription = "EFECTIVO", Status = "A" },
           //      new TipoMetodoDePago { TipoMetodoDePagoId = 11, TipoMetodoDePagoValue = "02", TipoMetodoDePagoCode = "02", TipoMetodoDePagoDescription = "CHEQUE NOMINATIVO", Status = "A" },
           //      new TipoMetodoDePago { TipoMetodoDePagoId = 12, TipoMetodoDePagoValue = "03", TipoMetodoDePagoCode = "03", TipoMetodoDePagoDescription = "TRANSFERENCIA ELECTRÓNICA DE FONDOS", Status = "A" },
           //      new TipoMetodoDePago { TipoMetodoDePagoId = 13, TipoMetodoDePagoValue = "04", TipoMetodoDePagoCode = "04", TipoMetodoDePagoDescription = "TARJETA DE CRÉDITO", Status = "A" },
           //      new TipoMetodoDePago { TipoMetodoDePagoId = 14, TipoMetodoDePagoValue = "05", TipoMetodoDePagoCode = "05", TipoMetodoDePagoDescription = "MONEDERO ELECTRÓNICO", Status = "A" },
           //      new TipoMetodoDePago { TipoMetodoDePagoId = 15, TipoMetodoDePagoValue = "06", TipoMetodoDePagoCode = "06", TipoMetodoDePagoDescription = "DINERO ELECTRÓNICO", Status = "A" },
           //      new TipoMetodoDePago { TipoMetodoDePagoId = 16, TipoMetodoDePagoValue = "08", TipoMetodoDePagoCode = "08", TipoMetodoDePagoDescription = "VALES DE DESPENSA", Status = "A" },
           //      new TipoMetodoDePago { TipoMetodoDePagoId = 17, TipoMetodoDePagoValue = "28", TipoMetodoDePagoCode = "28", TipoMetodoDePagoDescription = "TARJETA DE DÉBITO", Status = "A" },
           //      new TipoMetodoDePago { TipoMetodoDePagoId = 18, TipoMetodoDePagoValue = "29", TipoMetodoDePagoCode = "29", TipoMetodoDePagoDescription = "TARJETA DE SERVICIO", Status = "A" },
           //      new TipoMetodoDePago { TipoMetodoDePagoId = 19, TipoMetodoDePagoValue = "99", TipoMetodoDePagoCode = "99", TipoMetodoDePagoDescription = "OTROS", Status = "A" }
           //);
           // context.SaveChanges();

            context.TiposMoneda.AddOrUpdate(
                t => t.TipoMonedaValue,
                 new TipoMoneda { TipoMonedaId = 1, TipoMonedaValue = "MXN" },
                 new TipoMoneda { TipoMonedaId = 2, TipoMonedaValue = "USD" }
                );
            context.SaveChanges();

            //context.Bancos.AddOrUpdate(
            //    b => b.Clave,
            //    new Banco { BancoId = 1, PublicKey = new Guid("82E2C8AC-06AE-4F61-AFA6-364C731EF65D"), Clave = "002", NombreCorto = "BANAMEX", RazonSocial = "Banco Nacional de México, S.A., Institución de Banca Múltiple, Grupo Financiero Banamex", Status = "A" },
            //    new Banco { BancoId = 2, PublicKey = new Guid("777AA59F-B61D-47C8-999F-C594670902DE"), Clave = "006", NombreCorto = "BANCOMEXT", RazonSocial = "Banco Nacional de Comercio Exterior, Sociedad Nacional de Crédito, Institución de Banca de Desarrollo", Status = "A" }
            //);
            //context.SaveChanges();


            context.TiposFormaDePago.AddOrUpdate(
                t => t.TipoFormaDePagoValue,
                new TipoFormaDePago { TipoFormaDePagoId = 1, TipoFormaDePagoValue = "PAGO EN UNA SOLA EXHIBICION" }
            );
            context.SaveChanges();

            context.TiposImpuestos.AddOrUpdate(
                t => t.Impuesto,
                new TipoImpuesto { TipoImpuestoId = 1, Impuesto = "001", Descripcion = "ISR", Retencion = true, Traslado = false },
                new TipoImpuesto { TipoImpuestoId = 2, Impuesto = "002", Descripcion = "IVA", Retencion = true, Traslado = true },
                new TipoImpuesto { TipoImpuestoId = 3, Impuesto = "003", Descripcion = "IEPS", Retencion = true, Traslado = true }
            );
            context.SaveChanges();

            context.TiposFormaPago.AddOrUpdate(
                t => t.FormaPago,
                new TipoFormaPago { TipoFormaPagoId = 1, FormaPago = "99", Descripcion = "Por definir", Bancarizado = false },
                new TipoFormaPago { TipoFormaPagoId = 2, FormaPago = "01", Descripcion = "Efectivo", Bancarizado = false },
                new TipoFormaPago { TipoFormaPagoId = 3, FormaPago = "02", Descripcion = "Cheque nominativo", Bancarizado = true },
                new TipoFormaPago { TipoFormaPagoId = 4, FormaPago = "03", Descripcion = "Transferencia electrónica de fondos", Bancarizado = true },
                new TipoFormaPago { TipoFormaPagoId = 5, FormaPago = "04", Descripcion = "Tarjeta de crédito", Bancarizado = true },
                new TipoFormaPago { TipoFormaPagoId = 6, FormaPago = "05", Descripcion = "Monedero electrónico", Bancarizado = true },
                new TipoFormaPago { TipoFormaPagoId = 7, FormaPago = "06", Descripcion = "Dinero electrónico", Bancarizado = true },
                new TipoFormaPago { TipoFormaPagoId = 8, FormaPago = "08", Descripcion = "Vales de despensa", Bancarizado = false },
                new TipoFormaPago { TipoFormaPagoId = 9, FormaPago = "12", Descripcion = "Dación en pago", Bancarizado = false },
                new TipoFormaPago { TipoFormaPagoId = 10, FormaPago = "13", Descripcion = "Pago por subrogación", Bancarizado = false },
                new TipoFormaPago { TipoFormaPagoId = 11, FormaPago = "14", Descripcion = "Pago por consignación", Bancarizado = false },
                new TipoFormaPago { TipoFormaPagoId = 12, FormaPago = "15", Descripcion = "Condonación", Bancarizado = false },
                new TipoFormaPago { TipoFormaPagoId = 13, FormaPago = "17", Descripcion = "Compensación", Bancarizado = false },
                new TipoFormaPago { TipoFormaPagoId = 14, FormaPago = "23", Descripcion = "Novación", Bancarizado = false },
                new TipoFormaPago { TipoFormaPagoId = 15, FormaPago = "24", Descripcion = "Confusión", Bancarizado = false },
                new TipoFormaPago { TipoFormaPagoId = 16, FormaPago = "25", Descripcion = "Remisión de deuda", Bancarizado = false },
                new TipoFormaPago { TipoFormaPagoId = 17, FormaPago = "26", Descripcion = "Prescripción o caducidad", Bancarizado = false },
                new TipoFormaPago { TipoFormaPagoId = 18, FormaPago = "27", Descripcion = "A satisfacción del acreedor", Bancarizado = false },
                new TipoFormaPago { TipoFormaPagoId = 19, FormaPago = "28", Descripcion = "Tarjeta de débito", Bancarizado = true },
                new TipoFormaPago { TipoFormaPagoId = 20, FormaPago = "29", Descripcion = "Tarjeta de servicios", Bancarizado = true },
                new TipoFormaPago { TipoFormaPagoId = 21, FormaPago = "30", Descripcion = "Aplicación de anticipos", Bancarizado = false },
                new TipoFormaPago { TipoFormaPagoId = 22, FormaPago = "31", Descripcion = "Intermediario pagos", Bancarizado = false }

            );
            context.SaveChanges();

            context.TiposMetodoPago.AddOrUpdate(
                t => t.MetodoPago,
                new TipoMetodoPago { TipoMetodoPagoId = 1, MetodoPago = "PUE", Descripcion = "Pago en una sola exhibición", FechaInicioVigencia = new DateTime(2017,1,1) },
                new TipoMetodoPago { TipoMetodoPagoId = 2, MetodoPago = "PPD", Descripcion = "Pago en parcialidades o diferido", FechaInicioVigencia = new DateTime(2017, 1, 1) }
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
