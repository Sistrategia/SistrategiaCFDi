namespace Sistrategia.SAT.CFDiWebSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SATSchema : DbMigration
    {
        public override void Up()
        {
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
                .ForeignKey("dbo.sat_ubicacion_fiscal", t => t.domicilio_fiscal_id)
                .ForeignKey("dbo.sat_ubicacion_fiscal", t => t.expedido_en_id)
                .Index(t => t.public_key)
                .Index(t => t.domicilio_fiscal_id)
                .Index(t => t.expedido_en_id);
            
            CreateTable(
                "dbo.sat_ubicacion_fiscal",
                c => new
                    {
                        ubicacion_fiscal_id = c.Int(nullable: false, identity: true),
                        public_key = c.Guid(nullable: false),
                        calle = c.String(maxLength: 256),
                        no_exterior = c.String(maxLength: 50),
                        no_interior = c.String(maxLength: 50),
                        colonia = c.String(maxLength: 50),
                        localidad = c.String(maxLength: 50),
                        referencia = c.String(maxLength: 256),
                        municipio = c.String(maxLength: 50),
                        estado = c.String(maxLength: 50),
                        pais = c.String(maxLength: 50),
                        codigo_postal = c.String(maxLength: 5),
                    })
                .PrimaryKey(t => t.ubicacion_fiscal_id)
                .Index(t => t.public_key);
            
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
                        pais = c.String(maxLength: 50),
                        codigo_postal = c.String(maxLength: 5),
                    })
                .PrimaryKey(t => t.ubicacion_id)
                .Index(t => t.public_key);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.sat_emisor", "expedido_en_id", "dbo.sat_ubicacion_fiscal");
            DropForeignKey("dbo.sat_emisor", "domicilio_fiscal_id", "dbo.sat_ubicacion_fiscal");
            DropIndex("dbo.sat_ubicacion", new[] { "public_key" });
            DropIndex("dbo.sat_ubicacion_fiscal", new[] { "public_key" });
            DropIndex("dbo.sat_emisor", new[] { "expedido_en_id" });
            DropIndex("dbo.sat_emisor", new[] { "domicilio_fiscal_id" });
            DropIndex("dbo.sat_emisor", new[] { "public_key" });
            DropTable("dbo.sat_ubicacion");
            DropTable("dbo.sat_ubicacion_fiscal");
            DropTable("dbo.sat_emisor");
        }
    }
}
