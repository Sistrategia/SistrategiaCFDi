namespace Sistrategia.SAT.CFDiWebSite.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class v33 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.sat_concepto_impuestos",
                c => new
                    {
                        impuestos_id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.impuestos_id);
            
            CreateTable(
                "dbo.sat_concepto_impuesto_retencion",
                c => new
                    {
                        retencion_id = c.Int(nullable: false, identity: true),
                        _base = c.Decimal(name: "base", nullable: false, precision: 18, scale: 2),
                        impuesto = c.String(),
                        tipo_factor = c.String(),
                        tasa_o_cuota = c.Decimal(precision: 18, scale: 2),
                        importe = c.Decimal(precision: 18, scale: 2),
                        ordinal = c.Int(),
                        impuesto_id = c.Int(),
                    })
                .PrimaryKey(t => t.retencion_id)
                .ForeignKey("dbo.sat_concepto_impuestos", t => t.impuesto_id)
                .Index(t => t.impuesto_id);
            
            CreateTable(
                "dbo.sat_concepto_impuesto_traslado",
                c => new
                    {
                        traslado_id = c.Int(nullable: false, identity: true),
                        _base = c.Decimal(name: "base", nullable: false, precision: 18, scale: 2),
                        impuesto = c.String(),
                        tipo_factor = c.String(),
                        tasa_o_cuota = c.Decimal(precision: 18, scale: 2),
                        importe = c.Decimal(precision: 18, scale: 2),
                        ordinal = c.Int(),
                        impuesto_id = c.Int(),
                    })
                .PrimaryKey(t => t.traslado_id)
                .ForeignKey("dbo.sat_concepto_impuestos", t => t.impuesto_id)
                .Index(t => t.impuesto_id);
            
            CreateTable(
                "dbo.sat_tipo_forma_pago",
                c => new
                    {
                        tipo_forma_pago_id = c.Int(nullable: false, identity: true),
                        forma_pago = c.String(maxLength: 2),
                        descripcion = c.String(maxLength: 128),
                        bancarizado = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "False")
                                },
                            }),
                    })
                .PrimaryKey(t => t.tipo_forma_pago_id);
            
            CreateTable(
                "dbo.sat_tipo_impuesto",
                c => new
                    {
                        tipo_impuesto_id = c.Int(nullable: false, identity: true),
                        impuesto = c.String(maxLength: 3),
                        descripcion = c.String(maxLength: 128),
                        retencion = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "False")
                                },
                            }),
                        traslado = c.Boolean(nullable: false,
                            annotations: new Dictionary<string, AnnotationValues>
                            {
                                { 
                                    "Default",
                                    new AnnotationValues(oldValue: null, newValue: "False")
                                },
                            }),
                    })
                .PrimaryKey(t => t.tipo_impuesto_id);
            
            CreateTable(
                "dbo.sat_tipo_metodo_pago",
                c => new
                    {
                        tipo_metodo_pago_id = c.Int(nullable: false, identity: true),
                        metodo_pago = c.String(maxLength: 3),
                        descripcion = c.String(maxLength: 128),
                        fecha_fin_vigencia = c.DateTime(),
                        FechaFinVigencia = c.DateTime(),
                    })
                .PrimaryKey(t => t.tipo_metodo_pago_id);
            
            AddColumn("dbo.sat_comprobante", "forma_pago", c => c.String(maxLength: 2));
            AddColumn("dbo.sat_comprobante", "metodo_pago", c => c.String(maxLength: 3));
            AddColumn("dbo.sat_comprobante", "confirmacion", c => c.String(maxLength: 5));
            AddColumn("dbo.sat_timbre_fiscal_digital", "rfc_prov_certif", c => c.String());
            AddColumn("dbo.sat_timbre_fiscal_digital", "leyenda", c => c.String());
            AddColumn("dbo.sat_concepto", "clave_prod_serv", c => c.String(maxLength: 10));
            AddColumn("dbo.sat_concepto", "clave_unidad", c => c.String(maxLength: 20));
            AddColumn("dbo.sat_concepto", "descuento", c => c.Decimal(precision: 18, scale: 6));
            AddColumn("dbo.sat_concepto", "motivo_descuento", c => c.String(maxLength: 2048));
            AddColumn("dbo.sat_concepto", "impuestos_id", c => c.Int());
            AddColumn("dbo.sat_regimen_fiscal", "regimen_fiscal_clave", c => c.String());
            AddColumn("dbo.sat_traslado", "tipo_factor", c => c.String());
            AddColumn("dbo.sat_traslado", "tasa_o_cuota", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.sat_receptor", "residencia_fiscal", c => c.String());
            AddColumn("dbo.sat_receptor", "num_reg_id_trib", c => c.String());
            AddColumn("dbo.sat_receptor", "uso_cfdi", c => c.String());
            AlterColumn("dbo.sat_comprobante", "forma_de_pago", c => c.String(maxLength: 256));
            AlterColumn("dbo.sat_concepto", "unidad", c => c.String(maxLength: 20));
            AlterColumn("dbo.sat_concepto", "no_identificacion", c => c.String(maxLength: 100));
            AlterColumn("dbo.sat_traslado", "tasa", c => c.Decimal(precision: 18, scale: 2));
            CreateIndex("dbo.sat_concepto", "impuestos_id");
            AddForeignKey("dbo.sat_concepto", "impuestos_id", "dbo.sat_concepto_impuestos", "impuestos_id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.sat_concepto", "impuestos_id", "dbo.sat_concepto_impuestos");
            DropForeignKey("dbo.sat_concepto_impuesto_traslado", "impuesto_id", "dbo.sat_concepto_impuestos");
            DropForeignKey("dbo.sat_concepto_impuesto_retencion", "impuesto_id", "dbo.sat_concepto_impuestos");
            DropIndex("dbo.sat_concepto_impuesto_traslado", new[] { "impuesto_id" });
            DropIndex("dbo.sat_concepto_impuesto_retencion", new[] { "impuesto_id" });
            DropIndex("dbo.sat_concepto", new[] { "impuestos_id" });
            AlterColumn("dbo.sat_traslado", "tasa", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.sat_concepto", "no_identificacion", c => c.String(maxLength: 256));
            AlterColumn("dbo.sat_concepto", "unidad", c => c.String(maxLength: 50));
            AlterColumn("dbo.sat_comprobante", "forma_de_pago", c => c.String(nullable: false, maxLength: 256));
            DropColumn("dbo.sat_receptor", "uso_cfdi");
            DropColumn("dbo.sat_receptor", "num_reg_id_trib");
            DropColumn("dbo.sat_receptor", "residencia_fiscal");
            DropColumn("dbo.sat_traslado", "tasa_o_cuota");
            DropColumn("dbo.sat_traslado", "tipo_factor");
            DropColumn("dbo.sat_regimen_fiscal", "regimen_fiscal_clave");
            DropColumn("dbo.sat_concepto", "impuestos_id");
            DropColumn("dbo.sat_concepto", "motivo_descuento");
            DropColumn("dbo.sat_concepto", "descuento");
            DropColumn("dbo.sat_concepto", "clave_unidad");
            DropColumn("dbo.sat_concepto", "clave_prod_serv");
            DropColumn("dbo.sat_timbre_fiscal_digital", "leyenda");
            DropColumn("dbo.sat_timbre_fiscal_digital", "rfc_prov_certif");
            DropColumn("dbo.sat_comprobante", "confirmacion");
            DropColumn("dbo.sat_comprobante", "metodo_pago");
            DropColumn("dbo.sat_comprobante", "forma_pago");
            DropTable("dbo.sat_tipo_metodo_pago");
            DropTable("dbo.sat_tipo_impuesto",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "retencion",
                        new Dictionary<string, object>
                        {
                            { "Default", "False" },
                        }
                    },
                    {
                        "traslado",
                        new Dictionary<string, object>
                        {
                            { "Default", "False" },
                        }
                    },
                });
            DropTable("dbo.sat_tipo_forma_pago",
                removedColumnAnnotations: new Dictionary<string, IDictionary<string, object>>
                {
                    {
                        "bancarizado",
                        new Dictionary<string, object>
                        {
                            { "Default", "False" },
                        }
                    },
                });
            DropTable("dbo.sat_concepto_impuesto_traslado");
            DropTable("dbo.sat_concepto_impuesto_retencion");
            DropTable("dbo.sat_concepto_impuestos");
        }
    }
}
