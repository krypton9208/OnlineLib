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
                        Library_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Library", t => t.Library_Id)
                .Index(t => t.Library_Id);
            
            CreateTable(
                "dbo.Library",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nazwa = c.String(name: "Nazwa: ", nullable: false, maxLength: 200),
                        Zdjęcie = c.String(name: "Zdjęcie: ", maxLength: 200),
                        Tekst = c.String(name: "Tekst: ", maxLength: 1500),
                        Address_Id = c.Guid(),
                        BooksId = c.Int(),
                        Owner_Id = c.Guid(),
                        ReadersId = c.Guid(),
                        WorkersId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.Address_Id)
                .ForeignKey("dbo.Books", t => t.BooksId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.Owner_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ReadersId)
                .ForeignKey("dbo.AspNetUsers", t => t.WorkersId)
                .Index(t => t.Address_Id)
                .Index(t => t.BooksId)
                .Index(t => t.Owner_Id)
                .Index(t => t.ReadersId)
                .Index(t => t.WorkersId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Imie = c.String(name: "Imie: ", nullable: false, maxLength: 30),
                        Nazwisko = c.String(name: "Nazwisko: ", nullable: false, maxLength: 30),
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
                        Adress_Id = c.Guid(),
                        BookedBooks = c.Int(),
                        Library = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Addresses", t => t.Adress_Id)
                .ForeignKey("dbo.Books", t => t.BookedBooks, cascadeDelete: true)
                .ForeignKey("dbo.Library", t => t.Library)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex")
                .Index(t => t.Adress_Id)
                .Index(t => t.BookedBooks)
                .Index(t => t.Library);
            
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
            DropForeignKey("dbo.Books", "Library_Id", "dbo.Library");
            DropForeignKey("dbo.Library", "WorkersId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Library", "ReadersId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Library", "Owner_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Library", "dbo.Library");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "BookedBooks", "dbo.Books");
            DropForeignKey("dbo.AspNetUsers", "Adress_Id", "dbo.Addresses");
            DropForeignKey("dbo.Library", "BooksId", "dbo.Books");
            DropForeignKey("dbo.Library", "Address_Id", "dbo.Addresses");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", new[] { "Library" });
            DropIndex("dbo.AspNetUsers", new[] { "BookedBooks" });
            DropIndex("dbo.AspNetUsers", new[] { "Adress_Id" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Library", new[] { "WorkersId" });
            DropIndex("dbo.Library", new[] { "ReadersId" });
            DropIndex("dbo.Library", new[] { "Owner_Id" });
            DropIndex("dbo.Library", new[] { "BooksId" });
            DropIndex("dbo.Library", new[] { "Address_Id" });
            DropIndex("dbo.Books", new[] { "Library_Id" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Library");
            DropTable("dbo.Books");
            DropTable("dbo.Addresses");
        }
    }
}
