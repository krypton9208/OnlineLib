namespace OnlineLib.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class First : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Contry = c.String(),
                        City = c.String(),
                        Street = c.String(),
                        Number = c.String(),
                        PostCode = c.String(),
                        LibUserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Imie = c.String(name: "Imie: ", nullable: false, maxLength: 30),
                        Nazwisko = c.String(name: "Nazwisko: ", nullable: false, maxLength: 30),
                        AdresId = c.Guid(nullable: false),
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
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        Id = c.Int(name: "Id: ", nullable: false, identity: true),
                        Title = c.String(name: "Title: ", nullable: false, maxLength: 100),
                        Author = c.String(name: "Author: ", maxLength: 100),
                        Lended = c.Boolean(name: "Lended: "),
                        Isbn = c.String(name: "Isbn: ", maxLength: 16),
                        LibraryId = c.Int(),
                        BorrowerGuid = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.BorrowerGuid)
                .ForeignKey("dbo.Library", t => t.LibraryId)
                .Index(t => t.LibraryId)
                .Index(t => t.BorrowerGuid);
            
            CreateTable(
                "dbo.Library",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nazwa = c.String(name: "Nazwa: ", nullable: false, maxLength: 200),
                        Zdjęcie = c.String(name: "Zdjęcie: ", maxLength: 200),
                        AdresId = c.Guid(),
                        Tekst = c.String(name: "Tekst: ", maxLength: 1500),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.AdresId, cascadeDelete: true)
                .Index(t => t.AdresId);
            
            CreateTable(
                "dbo.LibUserLibrary",
                c => new
                    {
                        LibUserId = c.Guid(nullable: false),
                        LibraryId = c.Int(nullable: false),
                        Library = c.Int(nullable: false),
                        LibUser = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.LibUserId, t.LibraryId })
                .ForeignKey("dbo.Library", t => t.Library, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.LibUser, cascadeDelete: true)
                .Index(t => t.Library)
                .Index(t => t.LibUser);
            
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
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.Guid(nullable: false),
                        RoleId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.LibUserLibrary", "LibUser", "dbo.AspNetUsers");
            DropForeignKey("dbo.LibUserLibrary", "Library", "dbo.Library");
            DropForeignKey("dbo.Books", "LibraryId", "dbo.Library");
            DropForeignKey("dbo.Library", "AdresId", "dbo.Addresses");
            DropForeignKey("dbo.Books", "BorrowerGuid", "dbo.AspNetUsers");
            DropForeignKey("dbo.Addresses", "Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.LibUserLibrary", new[] { "LibUser" });
            DropIndex("dbo.LibUserLibrary", new[] { "Library" });
            DropIndex("dbo.Library", new[] { "AdresId" });
            DropIndex("dbo.Books", new[] { "BorrowerGuid" });
            DropIndex("dbo.Books", new[] { "LibraryId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Addresses", new[] { "Id" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.LibUserLibrary");
            DropTable("dbo.Library");
            DropTable("dbo.Books");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Addresses");
        }
    }
}
