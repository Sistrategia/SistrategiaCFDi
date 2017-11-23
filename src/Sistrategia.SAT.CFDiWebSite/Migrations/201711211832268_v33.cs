namespace Sistrategia.SAT.CFDiWebSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v33 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.sat_comprobante", "forma_pago", c => c.String(maxLength: 2));
            AddColumn("dbo.sat_comprobante", "metodo_pago", c => c.String(maxLength: 3));
            AddColumn("dbo.sat_comprobante", "confirmacion", c => c.String(maxLength: 5));
            AlterColumn("dbo.sat_comprobante", "forma_de_pago", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.sat_comprobante", "forma_de_pago", c => c.String(nullable: false, maxLength: 256));
            DropColumn("dbo.sat_comprobante", "confirmacion");
            DropColumn("dbo.sat_comprobante", "metodo_pago");
            DropColumn("dbo.sat_comprobante", "forma_pago");
        }
    }
}