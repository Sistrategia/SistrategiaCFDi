namespace Sistrategia.SAT.CFDiWebSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SATSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.sat_certificado",
                c => new
                    {
                        certificado_id = c.Int(nullable: false, identity: true),
                        public_key = c.Guid(nullable: false),
                        num_serie = c.String(nullable: false, maxLength: 20),
                        rfc = c.String(nullable: false, maxLength: 13),
                        inicia = c.DateTime(nullable: false),
                        finaliza = c.DateTime(nullable: false),
                        certificado = c.String(nullable: false),
                        pfx_archivo = c.Binary(),
                        pfx_contrasena = c.String(maxLength: 2048),
                        estado = c.String(nullable: false),
                        emisor_id = c.Int(),
                    })
                .PrimaryKey(t => t.certificado_id)
                .ForeignKey("dbo.sat_emisor", t => t.emisor_id)
                .Index(t => t.public_key)
                .Index(t => t.emisor_id);
            
            CreateTable(
                "dbo.sat_comprobante",
                c => new
                    {
                        comprobante_id = c.Int(nullable: false, identity: true),
                        public_key = c.Guid(nullable: false),
                        version = c.String(nullable: false, maxLength: 20),
                        serie = c.String(maxLength: 25),
                        folio = c.String(maxLength: 20),
                        fecha = c.DateTime(nullable: false),
                        sello = c.String(maxLength: 2048),
                        forma_de_pago = c.String(nullable: false, maxLength: 256),
                        no_certificado = c.String(maxLength: 20),
                        certificado = c.String(maxLength: 2048),
                        condiciones_de_pago = c.String(maxLength: 2048),
                        sub_total = c.Decimal(nullable: false, precision: 18, scale: 6),
                        descuento = c.Decimal(precision: 18, scale: 6),
                        motivo_descuento = c.String(maxLength: 2048),
                        tipo_cambio = c.String(maxLength: 50),
                        moneda = c.String(maxLength: 50),
                        total = c.Decimal(nullable: false, precision: 18, scale: 6),
                        tipo_de_comprobante = c.String(nullable: false, maxLength: 50),
                        metodo_de_pago = c.String(nullable: false, maxLength: 256),
                        lugar_expedicion = c.String(nullable: false, maxLength: 2048),
                        num_cta_pago = c.String(maxLength: 256),
                        folio_fiscal_orig = c.String(maxLength: 256),
                        serie_folio_fiscal_orig = c.String(maxLength: 256),
                        fecha_folio_fiscal_orig = c.DateTime(),
                        monto_folio_fiscal_orig = c.Decimal(precision: 18, scale: 6),
                        emisor_id = c.Int(),
                        receptor_id = c.Int(),
                    })
                .PrimaryKey(t => t.comprobante_id)
                .ForeignKey("dbo.sat_emisor", t => t.emisor_id)
                .ForeignKey("dbo.sat_receptor", t => t.receptor_id)
                .Index(t => t.public_key)
                .Index(t => t.emisor_id)
                .Index(t => t.receptor_id);
            
            CreateTable(
                "dbo.sat_concepto",
                c => new
                    {
                        concepto_id = c.Int(nullable: false, identity: true),
                        public_key = c.Guid(nullable: false),
                        cantidad = c.Decimal(nullable: false, precision: 18, scale: 6),
                        unidad = c.String(maxLength: 50),
                        no_identificacion = c.String(maxLength: 256),
                        descripcion = c.String(),
                        valor_unitario = c.Decimal(nullable: false, precision: 18, scale: 6),
                        importe = c.Decimal(nullable: false, precision: 18, scale: 6),
                        comprobante_id = c.Int(),
                    })
                .PrimaryKey(t => t.concepto_id)
                .ForeignKey("dbo.sat_comprobante", t => t.comprobante_id)
                .Index(t => t.public_key)
                .Index(t => t.comprobante_id);
            
            CreateTable(
                "dbo.sat_emisor",
                c => new
                    {
                        emisor_id = c.Int(nullable: false, identity: true),
                        public_key = c.Guid(nullable: false),
                        rfc = c.String(nullable: false, maxLength: 13),
                        nombre = c.String(maxLength: 256),
                        domicilio_fiscal_id = c.Int(),
                        expedido_en_id = c.Int(),
                    })
                .PrimaryKey(t => t.emisor_id)
                .ForeignKey("dbo.sat_ubicacion", t => t.domicilio_fiscal_id)
                .ForeignKey("dbo.sat_ubicacion", t => t.expedido_en_id)
                .Index(t => t.public_key)
                .Index(t => t.domicilio_fiscal_id)
                .Index(t => t.expedido_en_id);
            
            CreateTable(
                "dbo.sat_ubicacion",
                c => new
                    {
                        ubicacion_id = c.Int(nullable: false, identity: true),
                        public_key = c.Guid(nullable: false),
                        calle = c.String(maxLength: 256),
                        no_exterior = c.String(maxLength: 50),
                        no_interior = c.String(maxLength: 50),
                        colonia = c.String(maxLength: 50),
                        localidad = c.String(maxLength: 50),
                        referencia = c.String(maxLength: 256),
                        municipio = c.String(maxLength: 50),
                        estado = c.String(maxLength: 50),
                        pais = c.String(nullable: false, maxLength: 50),
                        codigo_postal = c.String(maxLength: 5),
                        ubicacion_type = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ubicacion_id)
                .Index(t => t.public_key);
            
            CreateTable(
                "dbo.sat_regimen_fiscal",
                c => new
                    {
                        regimen_fiscal_id = c.Int(nullable: false, identity: true),
                        regimen = c.String(),
                        emisor_id = c.Int(),
                    })
                .PrimaryKey(t => t.regimen_fiscal_id)
                .ForeignKey("dbo.sat_emisor", t => t.emisor_id)
                .Index(t => t.emisor_id);
            
            CreateTable(
                "dbo.sat_receptor",
                c => new
                    {
                        receptor_id = c.Int(nullable: false, identity: true),
                        public_key = c.Guid(nullable: false),
                        rfc = c.String(),
                        nombre = c.String(),
                        domicilio_id = c.Int(),
                    })
                .PrimaryKey(t => t.receptor_id)
                .ForeignKey("dbo.sat_ubicacion", t => t.domicilio_id)
                .Index(t => t.public_key)
                .Index(t => t.domicilio_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.sat_comprobante", "receptor_id", "dbo.sat_receptor");
            DropForeignKey("dbo.sat_receptor", "domicilio_id", "dbo.sat_ubicacion");
            DropForeignKey("dbo.sat_comprobante", "emisor_id", "dbo.sat_emisor");
            DropForeignKey("dbo.sat_regimen_fiscal", "emisor_id", "dbo.sat_emisor");
            DropForeignKey("dbo.sat_emisor", "expedido_en_id", "dbo.sat_ubicacion");
            DropForeignKey("dbo.sat_emisor", "domicilio_fiscal_id", "dbo.sat_ubicacion");
            DropForeignKey("dbo.sat_certificado", "emisor_id", "dbo.sat_emisor");
            DropForeignKey("dbo.sat_concepto", "comprobante_id", "dbo.sat_comprobante");
            DropIndex("dbo.sat_receptor", new[] { "domicilio_id" });
            DropIndex("dbo.sat_receptor", new[] { "public_key" });
            DropIndex("dbo.sat_regimen_fiscal", new[] { "emisor_id" });
            DropIndex("dbo.sat_ubicacion", new[] { "public_key" });
            DropIndex("dbo.sat_emisor", new[] { "expedido_en_id" });
            DropIndex("dbo.sat_emisor", new[] { "domicilio_fiscal_id" });
            DropIndex("dbo.sat_emisor", new[] { "public_key" });
            DropIndex("dbo.sat_concepto", new[] { "comprobante_id" });
            DropIndex("dbo.sat_concepto", new[] { "public_key" });
            DropIndex("dbo.sat_comprobante", new[] { "receptor_id" });
            DropIndex("dbo.sat_comprobante", new[] { "emisor_id" });
            DropIndex("dbo.sat_comprobante", new[] { "public_key" });
            DropIndex("dbo.sat_certificado", new[] { "emisor_id" });
            DropIndex("dbo.sat_certificado", new[] { "public_key" });
            DropTable("dbo.sat_receptor");
            DropTable("dbo.sat_regimen_fiscal");
            DropTable("dbo.sat_ubicacion");
            DropTable("dbo.sat_emisor");
            DropTable("dbo.sat_concepto");
            DropTable("dbo.sat_comprobante");
            DropTable("dbo.sat_certificado");
        }
    }
}
