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
                        Id = c.Int(nullable: false),
                        Contry = c.String(),
                        City = c.String(),
                        Street = c.String(),
                        Number = c.String(),
                        PostCode = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Library", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Library",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nazwa = c.String(name: "Nazwa: ", nullable: false, maxLength: 200),
                        Zdjęcie = c.String(name: "Zdjęcie: ", maxLength: 200),
                        Tekst = c.String(name: "Tekst: ", maxLength: 1500),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(name: "Title: ", nullable: false, maxLength: 100),
                        Author = c.String(name: "Author: ", maxLength: 100),
                        Lended = c.Boolean(name: "Lended: "),
                        Isbn = c.String(name: "Isbn: ", maxLength: 16),
                        LibraryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Library", t => t.LibraryId, cascadeDelete: true)
                .Index(t => t.LibraryId);
            
            CreateTable(
                "dbo.LoanActivities",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        LoanData = c.DateTime(nullable: false),
                        ReturnedData = c.DateTime(),
                        ScheduledReturnData = c.DateTime(nullable: false),
                        Returned = c.Boolean(),
                        Text = c.String(maxLength: 150),
                        BookId = c.Int(nullable: false),
                        LibUserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Books", t => t.BookId)
                .ForeignKey("dbo.AspNetUsers", t => t.LibUserId, cascadeDelete: true)
                .Index(t => t.BookId)
                .Index(t => t.LibUserId);
            
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
                        Adress_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.Adress_Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Adress_Id);
            
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
            
            CreateTable(
                "dbo.LibUserLibrary",
                c => new
                    {
                        LibUser_Id = c.Guid(nullable: false),
                        Library_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LibUser_Id, t.Library_Id })
                .ForeignKey("dbo.AspNetUsers", t => t.LibUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Library", t => t.Library_Id, cascadeDelete: true)
                .Index(t => t.LibUser_Id)
                .Index(t => t.Library_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.LoanActivities", "LibUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.LibUserLibrary", "Library_Id", "dbo.Library");
            DropForeignKey("dbo.LibUserLibrary", "LibUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Adress_Id", "dbo.Addresses");
            DropForeignKey("dbo.LoanActivities", "BookId", "dbo.Books");
            DropForeignKey("dbo.Books", "LibraryId", "dbo.Library");
            DropForeignKey("dbo.Addresses", "Id", "dbo.Library");
            DropIndex("dbo.LibUserLibrary", new[] { "Library_Id" });
            DropIndex("dbo.LibUserLibrary", new[] { "LibUser_Id" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Adress_Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.LoanActivities", new[] { "LibUserId" });
            DropIndex("dbo.LoanActivities", new[] { "BookId" });
            DropIndex("dbo.Books", new[] { "LibraryId" });
            DropIndex("dbo.Addresses", new[] { "Id" });
            DropTable("dbo.LibUserLibrary");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.LoanActivities");
            DropTable("dbo.Books");
            DropTable("dbo.Library");
            DropTable("dbo.Addresses");
        }
    }
}
