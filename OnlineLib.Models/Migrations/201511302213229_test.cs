namespace OnlineLib.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Contry = c.String(nullable: false, maxLength: 100),
                        City = c.String(nullable: false, maxLength: 100),
                        Street = c.String(nullable: false, maxLength: 150),
                        Number = c.String(nullable: false, maxLength: 6),
                        PostCode = c.String(nullable: false, maxLength: 6),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Autor = c.String(maxLength: 100),
                        Lended = c.Boolean(),
                        Isbn = c.String(maxLength: 16),
                        LibraryId = c.Int(nullable: false),
                        LibUser_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Library", t => t.LibraryId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.LibUser_Id)
                .Index(t => t.LibraryId)
                .Index(t => t.LibUser_Id);
            
            CreateTable(
                "dbo.Library",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nazwa = c.String(name: "Nazwa: ", nullable: false, maxLength: 200),
                        Zdjęcie = c.String(name: "Zdjęcie: ", maxLength: 200),
                        AdresId = c.Guid(nullable: false),
                        LibUsers = c.Guid(nullable: false),
                        Tekst = c.String(name: "Tekst: ", maxLength: 1500),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.AdresId, cascadeDelete: true)
                .Index(t => t.AdresId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Imie = c.String(name: "Imie: ", nullable: false, maxLength: 30),
                        Nazwisko = c.String(name: "Nazwisko: ", nullable: false, maxLength: 30),
                        AdresId = c.Guid(nullable: false),
                        Libraries = c.Int(nullable: false),
                        Email = c.String(name: "Email: ", nullable: false, maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.AdresId, cascadeDelete: true)
                .Index(t => t.AdresId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Guid(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.LibraryLibUsers",
                c => new
                    {
                        LibraryId = c.Int(nullable: false),
                        LibUserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.LibraryId, t.LibUserId })
                .ForeignKey("dbo.Library", t => t.LibraryId, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.LibUserId, cascadeDelete: false)
                .Index(t => t.LibraryId)
                .Index(t => t.LibUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LibraryLibUsers", "LibUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.LibraryLibUsers", "LibraryId", "dbo.Library");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Books", "LibUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "AdresId", "dbo.Addresses");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Books", "LibraryId", "dbo.Library");
            DropForeignKey("dbo.Library", "AdresId", "dbo.Addresses");
            DropIndex("dbo.LibraryLibUsers", new[] { "LibUserId" });
            DropIndex("dbo.LibraryLibUsers", new[] { "LibraryId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "AdresId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Library", new[] { "AdresId" });
            DropIndex("dbo.Books", new[] { "LibUser_Id" });
            DropIndex("dbo.Books", new[] { "LibraryId" });
            DropTable("dbo.LibraryLibUsers");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Library");
            DropTable("dbo.Books");
            DropTable("dbo.Addresses");
        }
    }
}
