namespace Sistrategia.SAT.CFDiWebSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.security_roles",
                c => new
                    {
                        role_id = c.Int(nullable: false, identity: true),
                        role_name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.role_id)
                .Index(t => t.role_name, unique: true, name: "ix_role_name_index");
            
            CreateTable(
                "dbo.security_user_roles",
                c => new
                    {
                        user_id = c.Int(nullable: false),
                        role_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.user_id, t.role_id })
                .ForeignKey("dbo.security_roles", t => t.role_id, cascadeDelete: true)
                .ForeignKey("dbo.security_user", t => t.user_id, cascadeDelete: true)
                .Index(t => t.user_id)
                .Index(t => t.role_id);
            
            CreateTable(
                "dbo.security_user",
                c => new
                    {
                        user_id = c.Int(nullable: false, identity: true),
                        user_name = c.String(nullable: false, maxLength: 256),
                        public_key = c.Guid(nullable: false),
                        full_name = c.String(maxLength: 256),
                        email = c.String(maxLength: 256),
                        email_confirmed = c.Boolean(nullable: false),
                        password_hash = c.String(),
                        security_stamp = c.String(),
                        phone_number = c.String(),
                        phone_number_confirmed = c.Boolean(nullable: false),
                        two_factor_enabled = c.Boolean(nullable: false),
                        lockout_end_date_utc = c.DateTime(),
                        lockout_enabled = c.Boolean(nullable: false),
                        access_failed_count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.user_id)
                .Index(t => t.user_name, unique: true, name: "ix_user_name_index")
                .Index(t => t.public_key, unique: true, name: "ix_user_public_key_index")
                .Index(t => t.full_name, name: "ix_user_full_name_index");
            
            CreateTable(
                "dbo.security_user_claims",
                c => new
                    {
                        claim_id = c.Int(nullable: false, identity: true),
                        user_id = c.Int(nullable: false),
                        claim_type = c.String(),
                        claim_value = c.String(),
                    })
                .PrimaryKey(t => t.claim_id)
                .ForeignKey("dbo.security_user", t => t.user_id, cascadeDelete: true)
                .Index(t => t.user_id);
            
            CreateTable(
                "dbo.security_user_logins",
                c => new
                    {
                        login_provider = c.String(nullable: false, maxLength: 128),
                        provider_key = c.String(nullable: false, maxLength: 128),
                        user_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.login_provider, t.provider_key, t.user_id })
                .ForeignKey("dbo.security_user", t => t.user_id, cascadeDelete: true)
                .Index(t => t.user_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.security_user_roles", "user_id", "dbo.security_user");
            DropForeignKey("dbo.security_user_logins", "user_id", "dbo.security_user");
            DropForeignKey("dbo.security_user_claims", "user_id", "dbo.security_user");
            DropForeignKey("dbo.security_user_roles", "role_id", "dbo.security_roles");
            DropIndex("dbo.security_user_logins", new[] { "user_id" });
            DropIndex("dbo.security_user_claims", new[] { "user_id" });
            DropIndex("dbo.security_user", "ix_user_full_name_index");
            DropIndex("dbo.security_user", "ix_user_public_key_index");
            DropIndex("dbo.security_user", "ix_user_name_index");
            DropIndex("dbo.security_user_roles", new[] { "role_id" });
            DropIndex("dbo.security_user_roles", new[] { "user_id" });
            DropIndex("dbo.security_roles", "ix_role_name_index");
            DropTable("dbo.security_user_logins");
            DropTable("dbo.security_user_claims");
            DropTable("dbo.security_user");
            DropTable("dbo.security_user_roles");
            DropTable("dbo.security_roles");
        }
    }
}
