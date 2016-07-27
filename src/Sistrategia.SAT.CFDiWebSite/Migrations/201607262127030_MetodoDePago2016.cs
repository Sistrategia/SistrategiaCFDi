namespace Sistrategia.SAT.CFDiWebSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MetodoDePago2016 : DbMigration
    {
        public override void Up()
        {
            //sat_tipo_metodo_de_pago
            AddColumn("dbo.sat_comprobante", "tipo_metodo_de_pago_id", c => c.Int());
            AddColumn("dbo.sat_tipo_metodo_de_pago", "tipo_metodo_de_pago_code", c => c.String(maxLength: 6));
            AddColumn("dbo.sat_tipo_metodo_de_pago", "tipo_metodo_de_pago_description", c => c.String(maxLength: 50));
            AddColumn("dbo.sat_tipo_metodo_de_pago", "status", c => c.String(nullable: false, maxLength: 50));

            Sql("UPDATE [dbo].[sat_tipo_metodo_de_pago] SET [tipo_metodo_de_pago_description] = 'EFECTIVO', [status] = 'I' WHERE [tipo_metodo_de_pago_id] = 1");
            Sql("UPDATE [dbo].[sat_tipo_metodo_de_pago] SET [tipo_metodo_de_pago_description] = 'CHEQUE', [status] = 'I' WHERE [tipo_metodo_de_pago_id] = 2");
            Sql("UPDATE [dbo].[sat_tipo_metodo_de_pago] SET [tipo_metodo_de_pago_description] = 'TRANSFERENCIA INTERBANCARIA', [status] = 'I' WHERE [tipo_metodo_de_pago_id] = 3");
            Sql("UPDATE [dbo].[sat_tipo_metodo_de_pago] SET [tipo_metodo_de_pago_description] = 'NO IDENTIFICADO', [status] = 'I' WHERE [tipo_metodo_de_pago_id] = 4");
            Sql("UPDATE [dbo].[sat_tipo_metodo_de_pago] SET [tipo_metodo_de_pago_description] = 'TARJETA DE CRÉDITO', [status] = 'I' WHERE [tipo_metodo_de_pago_id] = 5");
            Sql("UPDATE [dbo].[sat_tipo_metodo_de_pago] SET [tipo_metodo_de_pago_description] = 'TARJETA DE DÉBITO', [status] = 'I' WHERE [tipo_metodo_de_pago_id] = 6");

            Sql("SET IDENTITY_INSERT [dbo].[sat_tipo_metodo_de_pago] ON");
            Sql("INSERT INTO [dbo].[sat_tipo_metodo_de_pago] ([tipo_metodo_de_pago_id], [tipo_metodo_de_pago_value], [tipo_metodo_de_pago_code], [tipo_metodo_de_pago_description], [status]) VALUES (7, 'DEPOSITO BANCARIO', NULL, 'DEPOSITO BANCARIO', 'I')");
            Sql("INSERT INTO [dbo].[sat_tipo_metodo_de_pago] ([tipo_metodo_de_pago_id], [tipo_metodo_de_pago_value], [tipo_metodo_de_pago_code], [tipo_metodo_de_pago_description], [status]) VALUES (8, 'TRANSFERENCIA', NULL, 'TRANSFERENCIA', 'I')");
            Sql("INSERT INTO [dbo].[sat_tipo_metodo_de_pago] ([tipo_metodo_de_pago_id], [tipo_metodo_de_pago_value], [tipo_metodo_de_pago_code], [tipo_metodo_de_pago_description], [status]) VALUES (9, 'No Identificado', NULL, 'No Identificado', 'I')");
            Sql("INSERT INTO [dbo].[sat_tipo_metodo_de_pago] ([tipo_metodo_de_pago_id], [tipo_metodo_de_pago_value], [tipo_metodo_de_pago_code], [tipo_metodo_de_pago_description], [status]) VALUES (10, '01', '01', 'EFECTIVO', 'A')");
            Sql("INSERT INTO [dbo].[sat_tipo_metodo_de_pago] ([tipo_metodo_de_pago_id], [tipo_metodo_de_pago_value], [tipo_metodo_de_pago_code], [tipo_metodo_de_pago_description], [status]) VALUES (11, '02', '02', 'CHEQUE NOMINATIVO', 'A')");
            Sql("INSERT INTO [dbo].[sat_tipo_metodo_de_pago] ([tipo_metodo_de_pago_id], [tipo_metodo_de_pago_value], [tipo_metodo_de_pago_code], [tipo_metodo_de_pago_description], [status]) VALUES (12, '03', '03', 'TRANSFERENCIA ELECTRÓNICA DE FONDOS', 'A')");
            Sql("INSERT INTO [dbo].[sat_tipo_metodo_de_pago] ([tipo_metodo_de_pago_id], [tipo_metodo_de_pago_value], [tipo_metodo_de_pago_code], [tipo_metodo_de_pago_description], [status]) VALUES (13, '04', '04', 'TARJETA DE CRÉDITO', 'A')");
            Sql("INSERT INTO [dbo].[sat_tipo_metodo_de_pago] ([tipo_metodo_de_pago_id], [tipo_metodo_de_pago_value], [tipo_metodo_de_pago_code], [tipo_metodo_de_pago_description], [status]) VALUES (14, '05', '05', 'MONEDERO ELECTRÓNICO', 'A')");
            Sql("INSERT INTO [dbo].[sat_tipo_metodo_de_pago] ([tipo_metodo_de_pago_id], [tipo_metodo_de_pago_value], [tipo_metodo_de_pago_code], [tipo_metodo_de_pago_description], [status]) VALUES (15, '06', '06', 'DINERO ELECTRÓNICO', 'A')");
            Sql("INSERT INTO [dbo].[sat_tipo_metodo_de_pago] ([tipo_metodo_de_pago_id], [tipo_metodo_de_pago_value], [tipo_metodo_de_pago_code], [tipo_metodo_de_pago_description], [status]) VALUES (16, '08', '08', 'VALES DE DESPENSA', 'A')");
            Sql("INSERT INTO [dbo].[sat_tipo_metodo_de_pago] ([tipo_metodo_de_pago_id], [tipo_metodo_de_pago_value], [tipo_metodo_de_pago_code], [tipo_metodo_de_pago_description], [status]) VALUES (17, '28', '28', 'TARJETA DE DÉBITO', 'A')");
            Sql("INSERT INTO [dbo].[sat_tipo_metodo_de_pago] ([tipo_metodo_de_pago_id], [tipo_metodo_de_pago_value], [tipo_metodo_de_pago_code], [tipo_metodo_de_pago_description], [status]) VALUES (18, '29', '29', 'TARJETA DE SERVICIO', 'A')");
            Sql("INSERT INTO [dbo].[sat_tipo_metodo_de_pago] ([tipo_metodo_de_pago_id], [tipo_metodo_de_pago_value], [tipo_metodo_de_pago_code], [tipo_metodo_de_pago_description], [status]) VALUES (19, '99', '99', 'OTROS', 'A')");
            Sql("SET IDENTITY_INSERT [dbo].[sat_tipo_metodo_de_pago] OFF");
            
            //sat_comprobante
            CreateIndex("dbo.sat_comprobante", "tipo_metodo_de_pago_id");

            Sql("UPDATE [sat_comprobante] SET tipo_metodo_de_pago_id = tmp.tipo_metodo_de_pago_id FROM [sat_comprobante] AS c INNER JOIN [sat_tipo_metodo_de_pago] AS tmp ON c.metodo_de_pago = tmp.tipo_metodo_de_pago_description");

            AddForeignKey("dbo.sat_comprobante", "tipo_metodo_de_pago_id", "dbo.sat_tipo_metodo_de_pago", "tipo_metodo_de_pago_id");
                       
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.sat_comprobante", "tipo_metodo_de_pago_id", "dbo.sat_tipo_metodo_de_pago");
            DropIndex("dbo.sat_comprobante", new[] { "tipo_metodo_de_pago_id" });
            DropColumn("dbo.sat_tipo_metodo_de_pago", "status");
            DropColumn("dbo.sat_tipo_metodo_de_pago", "tipo_metodo_de_pago_description");
            DropColumn("dbo.sat_tipo_metodo_de_pago", "tipo_metodo_de_pago_code");
            DropColumn("dbo.sat_comprobante", "tipo_metodo_de_pago_id");
        }
    }
}
