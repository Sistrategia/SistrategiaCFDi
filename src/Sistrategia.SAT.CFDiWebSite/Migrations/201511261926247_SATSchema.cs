namespace Sistrategia.SAT.CFDiWebSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SATSchema : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.sat_banco",
                c => new
                    {
                        banco_id = c.Int(nullable: false, identity: true),
                        public_key = c.Guid(nullable: false),
                        clave = c.String(nullable: false, maxLength: 4),
                        nombre_corto = c.String(nullable: false, maxLength: 50),
                        razon_social = c.String(nullable: false, maxLength: 256),
                        status = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.banco_id);
            
            CreateTable(
                "dbo.sat_cancelacion",
                c => new
                    {
                        cancelacion_id = c.Int(nullable: false, identity: true),
                        ack = c.String(),
                        text = c.String(),
                        cancelacion_xml_response_url = c.String(),
                    })
                .PrimaryKey(t => t.cancelacion_id);
            
            CreateTable(
                "dbo.sat_cancelacion_uuid_comprobantes",
                c => new
                    {
                        cancelacion_uuid_comprobantes_id = c.Int(nullable: false, identity: true),
                        cancelacion_id = c.Int(),
                        uuid = c.String(),
                        comprobante_id = c.Int(),
                    })
                .PrimaryKey(t => t.cancelacion_uuid_comprobantes_id)
                .ForeignKey("dbo.sat_cancelacion", t => t.cancelacion_id)
                .ForeignKey("dbo.sat_comprobante", t => t.comprobante_id)
                .Index(t => t.cancelacion_id)
                .Index(t => t.comprobante_id);
            
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
                        no_aprobacion = c.String(),
                        ano_aprobacion = c.String(),
                        forma_de_pago = c.String(nullable: false, maxLength: 256),
                        certificado_id = c.Int(),
                        has_no_certificado = c.Boolean(nullable: false),
                        has_certificado = c.Boolean(nullable: false),
                        condiciones_de_pago = c.String(maxLength: 2048),
                        sub_total = c.Decimal(nullable: false, precision: 18, scale: 6),
                        descuento = c.Decimal(precision: 18, scale: 6),
                        motivo_descuento = c.String(maxLength: 2048),
                        tipo_cambio = c.String(maxLength: 50),
                        moneda = c.String(maxLength: 50),
                        total = c.Decimal(nullable: false, precision: 18, scale: 6),
                        tipo_de_comprobante = c.String(nullable: false, maxLength: 50),
                        metodo_de_pago = c.String(maxLength: 256),
                        lugar_expedicion = c.String(maxLength: 2048),
                        num_cta_pago = c.String(maxLength: 256),
                        folio_fiscal_orig = c.String(maxLength: 256),
                        serie_folio_fiscal_orig = c.String(maxLength: 256),
                        fecha_folio_fiscal_orig = c.DateTime(),
                        monto_folio_fiscal_orig = c.Decimal(precision: 18, scale: 6),
                        comprobante_emisor_id = c.Int(),
                        comprobante_receptor_id = c.Int(),
                        impuestos_id = c.Int(),
                        generated_cadena_original = c.String(),
                        generated_xml_url = c.String(maxLength: 1024),
                        generated_pdf_url = c.String(maxLength: 1024),
                        view_template_id = c.Int(),
                        extended_int_value_1 = c.Int(),
                        extended_int_value_2 = c.Int(),
                        extended_int_value_3 = c.Int(),
                        extended_string_value_1 = c.String(),
                        extended_string_value_2 = c.String(),
                        extended_string_value_3 = c.String(),
                        status = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.comprobante_id)
                .ForeignKey("dbo.sat_certificado", t => t.certificado_id)
                .ForeignKey("dbo.sat_comprobante_emisor", t => t.comprobante_emisor_id)
                .ForeignKey("dbo.sat_impuestos", t => t.impuestos_id)
                .ForeignKey("dbo.sat_comprobante_receptor", t => t.comprobante_receptor_id)
                .ForeignKey("dbo.ui_view_template", t => t.view_template_id)
                .Index(t => t.public_key)
                .Index(t => t.certificado_id)
                .Index(t => t.comprobante_emisor_id)
                .Index(t => t.comprobante_receptor_id)
                .Index(t => t.impuestos_id)
                .Index(t => t.view_template_id);
            
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
                        pfx_contrasena = c.String(maxLength: 256),
                        certificado_der = c.Binary(),
                        private_key_der = c.Binary(),
                        private_key_contrasena = c.String(maxLength: 256),
                        estado = c.String(nullable: false),
                        ordinal = c.Int(nullable: false),
                        emisor_id = c.Int(),
                    })
                .PrimaryKey(t => t.certificado_id)
                .ForeignKey("dbo.sat_emisor", t => t.emisor_id)
                .Index(t => t.public_key)
                .Index(t => t.emisor_id);
            
            CreateTable(
                "dbo.sat_complemento",
                c => new
                    {
                        complemento_id = c.Int(nullable: false, identity: true),
                        ordinal = c.Int(),
                        comprobante_id = c.Int(),
                    })
                .PrimaryKey(t => t.complemento_id)
                .ForeignKey("dbo.sat_comprobante", t => t.comprobante_id)
                .Index(t => t.comprobante_id);
            
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
                        ordinal = c.Int(nullable: false),
                        comprobante_id = c.Int(),
                    })
                .PrimaryKey(t => t.concepto_id)
                .ForeignKey("dbo.sat_comprobante", t => t.comprobante_id)
                .Index(t => t.public_key)
                .Index(t => t.comprobante_id);
            
            CreateTable(
                "dbo.sat_receptor_correo_entrega",
                c => new
                    {
                        receptor_correo_entrega_id = c.Int(nullable: false, identity: true),
                        correo = c.String(maxLength: 256),
                        comprobante_id = c.Int(),
                    })
                .PrimaryKey(t => t.receptor_correo_entrega_id)
                .ForeignKey("dbo.sat_comprobante", t => t.comprobante_id)
                .Index(t => t.comprobante_id);
            
            CreateTable(
                "dbo.sat_comprobante_emisor",
                c => new
                    {
                        comprobante_emisor_id = c.Int(nullable: false, identity: true),
                        emisor_id = c.Int(nullable: false),
                        domicilio_fiscal_id = c.Int(),
                        expedido_en_id = c.Int(),
                    })
                .PrimaryKey(t => t.comprobante_emisor_id)
                .ForeignKey("dbo.sat_ubicacion", t => t.domicilio_fiscal_id)
                .ForeignKey("dbo.sat_emisor", t => t.emisor_id, cascadeDelete: true)
                .ForeignKey("dbo.sat_ubicacion", t => t.expedido_en_id)
                .Index(t => t.emisor_id)
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
                        lugar_expedicion = c.String(maxLength: 2048),
                        ordinal = c.Int(nullable: false),
                        status = c.String(maxLength: 50),
                        emisor_id = c.Int(),
                        ubicacion_type = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.ubicacion_id)
                .ForeignKey("dbo.sat_emisor", t => t.emisor_id)
                .Index(t => t.public_key)
                .Index(t => t.emisor_id);
            
            CreateTable(
                "dbo.sat_emisor",
                c => new
                    {
                        emisor_id = c.Int(nullable: false, identity: true),
                        public_key = c.Guid(nullable: false),
                        rfc = c.String(nullable: false, maxLength: 13),
                        nombre = c.String(maxLength: 256),
                        domicilio_fiscal_id = c.Int(),
                        telefono = c.String(),
                        correo = c.String(),
                        cif_url = c.String(),
                        logo_url = c.String(),
                        view_template_id = c.Int(),
                        status = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.emisor_id)
                .ForeignKey("dbo.sat_ubicacion", t => t.domicilio_fiscal_id)
                .ForeignKey("dbo.ui_view_template", t => t.view_template_id)
                .Index(t => t.public_key)
                .Index(t => t.domicilio_fiscal_id)
                .Index(t => t.view_template_id);
            
            CreateTable(
                "dbo.sat_regimen_fiscal",
                c => new
                    {
                        regimen_fiscal_id = c.Int(nullable: false, identity: true),
                        regimen = c.String(),
                        ordinal = c.Int(nullable: false),
                        status = c.String(maxLength: 50),
                        emisor_id = c.Int(),
                    })
                .PrimaryKey(t => t.regimen_fiscal_id)
                .ForeignKey("dbo.sat_emisor", t => t.emisor_id)
                .Index(t => t.emisor_id);
            
            CreateTable(
                "dbo.ui_view_template",
                c => new
                    {
                        view_template_id = c.Int(nullable: false, identity: true),
                        display_name = c.String(maxLength: 256),
                        description = c.String(maxLength: 2048),
                        code_name = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.view_template_id);
            
            CreateTable(
                "dbo.sat_impuestos",
                c => new
                    {
                        impuestos_id = c.Int(nullable: false, identity: true),
                        TotalImpuestosRetenidos = c.Decimal(precision: 18, scale: 2),
                        TotalImpuestosTrasladados = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.impuestos_id);
            
            CreateTable(
                "dbo.sat_retencion",
                c => new
                    {
                        retencion_id = c.Int(nullable: false, identity: true),
                        impuesto = c.String(),
                        importe = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ordinal = c.Int(),
                        impuesto_id = c.Int(),
                    })
                .PrimaryKey(t => t.retencion_id)
                .ForeignKey("dbo.sat_impuestos", t => t.impuesto_id)
                .Index(t => t.impuesto_id);
            
            CreateTable(
                "dbo.sat_traslado",
                c => new
                    {
                        traslado_id = c.Int(nullable: false, identity: true),
                        impuesto = c.String(),
                        tasa = c.Decimal(nullable: false, precision: 18, scale: 2),
                        importe = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ordinal = c.Int(),
                        impuesto_id = c.Int(),
                    })
                .PrimaryKey(t => t.traslado_id)
                .ForeignKey("dbo.sat_impuestos", t => t.impuesto_id)
                .Index(t => t.impuesto_id);
            
            CreateTable(
                "dbo.sat_comprobante_receptor",
                c => new
                    {
                        comprobante_receptor_id = c.Int(nullable: false, identity: true),
                        receptor_id = c.Int(nullable: false),
                        domicilio_id = c.Int(),
                    })
                .PrimaryKey(t => t.comprobante_receptor_id)
                .ForeignKey("dbo.sat_ubicacion", t => t.domicilio_id)
                .ForeignKey("dbo.sat_receptor", t => t.receptor_id, cascadeDelete: true)
                .Index(t => t.receptor_id)
                .Index(t => t.domicilio_id);
            
            CreateTable(
                "dbo.sat_receptor",
                c => new
                    {
                        receptor_id = c.Int(nullable: false, identity: true),
                        public_key = c.Guid(nullable: false),
                        rfc = c.String(),
                        nombre = c.String(),
                        domicilio_id = c.Int(),
                        status = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.receptor_id)
                .ForeignKey("dbo.sat_ubicacion", t => t.domicilio_id)
                .Index(t => t.public_key)
                .Index(t => t.domicilio_id);
            
            CreateTable(
                "dbo.sat_tipo_forma_de_pago",
                c => new
                    {
                        tipo_forma_de_pago_id = c.Int(nullable: false, identity: true),
                        tipo_forma_de_pago_value = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.tipo_forma_de_pago_id);
            
            CreateTable(
                "dbo.sat_tipo_impuesto_retencion",
                c => new
                    {
                        tipo_impuesto_retencion_id = c.Int(nullable: false, identity: true),
                        tipo_impuesto_retencion_value = c.String(maxLength: 4),
                    })
                .PrimaryKey(t => t.tipo_impuesto_retencion_id);
            
            CreateTable(
                "dbo.sat_tipo_impuesto_traslado",
                c => new
                    {
                        tipo_impuesto_traslado_id = c.Int(nullable: false, identity: true),
                        tipo_impuesto_traslado_value = c.String(maxLength: 4),
                    })
                .PrimaryKey(t => t.tipo_impuesto_traslado_id);
            
            CreateTable(
                "dbo.sat_tipo_metodo_de_pago",
                c => new
                    {
                        tipo_metodo_de_pago_id = c.Int(nullable: false, identity: true),
                        tipo_metodo_de_pago_value = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.tipo_metodo_de_pago_id);
            
            CreateTable(
                "dbo.sat_tipo_moneda",
                c => new
                    {
                        tipo_moneda_id = c.Int(nullable: false, identity: true),
                        tipo_moneda_value = c.String(maxLength: 4),
                    })
                .PrimaryKey(t => t.tipo_moneda_id);
            
            CreateTable(
                "dbo.sat_tipo_tipo_de_comprobante",
                c => new
                    {
                        tipo_tipo_de_comprobante_id = c.Int(nullable: false, identity: true),
                        tipo_tipo_de_comprobante_value = c.String(maxLength: 12),
                    })
                .PrimaryKey(t => t.tipo_tipo_de_comprobante_id);
            
            CreateTable(
                "dbo.sat_timbre_fiscal_digital",
                c => new
                    {
                        complemento_id = c.Int(nullable: false),
                        version = c.String(),
                        uuid = c.String(),
                        fecha_timbrado = c.DateTime(nullable: false),
                        sello_cfd = c.String(),
                        no_certificado_sat = c.String(),
                        sello_sat = c.String(),
                    })
                .PrimaryKey(t => t.complemento_id)
                .ForeignKey("dbo.sat_complemento", t => t.complemento_id)
                .Index(t => t.complemento_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.sat_timbre_fiscal_digital", "complemento_id", "dbo.sat_complemento");
            DropForeignKey("dbo.sat_cancelacion_uuid_comprobantes", "comprobante_id", "dbo.sat_comprobante");
            DropForeignKey("dbo.sat_comprobante", "view_template_id", "dbo.ui_view_template");
            DropForeignKey("dbo.sat_comprobante", "comprobante_receptor_id", "dbo.sat_comprobante_receptor");
            DropForeignKey("dbo.sat_comprobante_receptor", "receptor_id", "dbo.sat_receptor");
            DropForeignKey("dbo.sat_receptor", "domicilio_id", "dbo.sat_ubicacion");
            DropForeignKey("dbo.sat_comprobante_receptor", "domicilio_id", "dbo.sat_ubicacion");
            DropForeignKey("dbo.sat_comprobante", "impuestos_id", "dbo.sat_impuestos");
            DropForeignKey("dbo.sat_traslado", "impuesto_id", "dbo.sat_impuestos");
            DropForeignKey("dbo.sat_retencion", "impuesto_id", "dbo.sat_impuestos");
            DropForeignKey("dbo.sat_comprobante", "comprobante_emisor_id", "dbo.sat_comprobante_emisor");
            DropForeignKey("dbo.sat_comprobante_emisor", "expedido_en_id", "dbo.sat_ubicacion");
            DropForeignKey("dbo.sat_comprobante_emisor", "emisor_id", "dbo.sat_emisor");
            DropForeignKey("dbo.sat_emisor", "view_template_id", "dbo.ui_view_template");
            DropForeignKey("dbo.sat_regimen_fiscal", "emisor_id", "dbo.sat_emisor");
            DropForeignKey("dbo.sat_ubicacion", "emisor_id", "dbo.sat_emisor");
            DropForeignKey("dbo.sat_emisor", "domicilio_fiscal_id", "dbo.sat_ubicacion");
            DropForeignKey("dbo.sat_certificado", "emisor_id", "dbo.sat_emisor");
            DropForeignKey("dbo.sat_comprobante_emisor", "domicilio_fiscal_id", "dbo.sat_ubicacion");
            DropForeignKey("dbo.sat_receptor_correo_entrega", "comprobante_id", "dbo.sat_comprobante");
            DropForeignKey("dbo.sat_concepto", "comprobante_id", "dbo.sat_comprobante");
            DropForeignKey("dbo.sat_complemento", "comprobante_id", "dbo.sat_comprobante");
            DropForeignKey("dbo.sat_comprobante", "certificado_id", "dbo.sat_certificado");
            DropForeignKey("dbo.sat_cancelacion_uuid_comprobantes", "cancelacion_id", "dbo.sat_cancelacion");
            DropIndex("dbo.sat_timbre_fiscal_digital", new[] { "complemento_id" });
            DropIndex("dbo.sat_receptor", new[] { "domicilio_id" });
            DropIndex("dbo.sat_receptor", new[] { "public_key" });
            DropIndex("dbo.sat_comprobante_receptor", new[] { "domicilio_id" });
            DropIndex("dbo.sat_comprobante_receptor", new[] { "receptor_id" });
            DropIndex("dbo.sat_traslado", new[] { "impuesto_id" });
            DropIndex("dbo.sat_retencion", new[] { "impuesto_id" });
            DropIndex("dbo.sat_regimen_fiscal", new[] { "emisor_id" });
            DropIndex("dbo.sat_emisor", new[] { "view_template_id" });
            DropIndex("dbo.sat_emisor", new[] { "domicilio_fiscal_id" });
            DropIndex("dbo.sat_emisor", new[] { "public_key" });
            DropIndex("dbo.sat_ubicacion", new[] { "emisor_id" });
            DropIndex("dbo.sat_ubicacion", new[] { "public_key" });
            DropIndex("dbo.sat_comprobante_emisor", new[] { "expedido_en_id" });
            DropIndex("dbo.sat_comprobante_emisor", new[] { "domicilio_fiscal_id" });
            DropIndex("dbo.sat_comprobante_emisor", new[] { "emisor_id" });
            DropIndex("dbo.sat_receptor_correo_entrega", new[] { "comprobante_id" });
            DropIndex("dbo.sat_concepto", new[] { "comprobante_id" });
            DropIndex("dbo.sat_concepto", new[] { "public_key" });
            DropIndex("dbo.sat_complemento", new[] { "comprobante_id" });
            DropIndex("dbo.sat_certificado", new[] { "emisor_id" });
            DropIndex("dbo.sat_certificado", new[] { "public_key" });
            DropIndex("dbo.sat_comprobante", new[] { "view_template_id" });
            DropIndex("dbo.sat_comprobante", new[] { "impuestos_id" });
            DropIndex("dbo.sat_comprobante", new[] { "comprobante_receptor_id" });
            DropIndex("dbo.sat_comprobante", new[] { "comprobante_emisor_id" });
            DropIndex("dbo.sat_comprobante", new[] { "certificado_id" });
            DropIndex("dbo.sat_comprobante", new[] { "public_key" });
            DropIndex("dbo.sat_cancelacion_uuid_comprobantes", new[] { "comprobante_id" });
            DropIndex("dbo.sat_cancelacion_uuid_comprobantes", new[] { "cancelacion_id" });
            DropTable("dbo.sat_timbre_fiscal_digital");
            DropTable("dbo.sat_tipo_tipo_de_comprobante");
            DropTable("dbo.sat_tipo_moneda");
            DropTable("dbo.sat_tipo_metodo_de_pago");
            DropTable("dbo.sat_tipo_impuesto_traslado");
            DropTable("dbo.sat_tipo_impuesto_retencion");
            DropTable("dbo.sat_tipo_forma_de_pago");
            DropTable("dbo.sat_receptor");
            DropTable("dbo.sat_comprobante_receptor");
            DropTable("dbo.sat_traslado");
            DropTable("dbo.sat_retencion");
            DropTable("dbo.sat_impuestos");
            DropTable("dbo.ui_view_template");
            DropTable("dbo.sat_regimen_fiscal");
            DropTable("dbo.sat_emisor");
            DropTable("dbo.sat_ubicacion");
            DropTable("dbo.sat_comprobante_emisor");
            DropTable("dbo.sat_receptor_correo_entrega");
            DropTable("dbo.sat_concepto");
            DropTable("dbo.sat_complemento");
            DropTable("dbo.sat_certificado");
            DropTable("dbo.sat_comprobante");
            DropTable("dbo.sat_cancelacion_uuid_comprobantes");
            DropTable("dbo.sat_cancelacion");
            DropTable("dbo.sat_banco");
        }
    }
}
