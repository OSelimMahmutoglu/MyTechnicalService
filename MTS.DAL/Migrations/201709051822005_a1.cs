namespace MTS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArizaKayitDetaylari",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ArizaKayitId = c.Int(nullable: false),
                        OperatorId = c.String(maxLength: 128),
                        OperatorIslemZamani = c.DateTime(nullable: false),
                        OperatorOnayladiMi = c.Boolean(nullable: false),
                        OperatorAciklamasi = c.String(),
                        TeknisyenId = c.String(maxLength: 128),
                        TeknisyenRaporu = c.String(),
                        Fiyat = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BitisZamani = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ArizaKayitlari", t => t.ArizaKayitId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.OperatorId)
                .ForeignKey("dbo.AspNetUsers", t => t.TeknisyenId)
                .Index(t => t.ArizaKayitId)
                .Index(t => t.OperatorId)
                .Index(t => t.TeknisyenId);
            
            CreateTable(
                "dbo.ArizaKayitlari",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Adres = c.String(),
                        KullaniciId = c.String(maxLength: 128),
                        KategoriId = c.Int(nullable: false),
                        Aciklama = c.String(),
                        FotografYolu = c.String(),
                        Konum = c.String(),
                        Puan = c.Byte(nullable: false),
                        ArizaKayitZamani = c.DateTime(nullable: false),
                        ArizaGiderildiMi = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Kategoriler", t => t.KategoriId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.KullaniciId)
                .Index(t => t.KullaniciId)
                .Index(t => t.KategoriId);
            
            CreateTable(
                "dbo.Kategoriler",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        KategoriAdi = c.String(nullable: false, maxLength: 100),
                        Aciklama = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.KategoriAdi, unique: true);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        UserName = c.String(nullable: false, maxLength: 256),
                        Name = c.String(nullable: false, maxLength: 25),
                        Surname = c.String(nullable: false, maxLength: 35),
                        RegisterDate = c.DateTime(nullable: false, storeType: "smalldatetime"),
                        ActivationCode = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Email, unique: true)
                .Index(t => t.UserName, unique: true)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
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
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
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
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Description = c.String(maxLength: 200),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ArizaKayitDetaylari", "TeknisyenId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ArizaKayitDetaylari", "OperatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ArizaKayitlari", "KullaniciId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ArizaKayitlari", "KategoriId", "dbo.Kategoriler");
            DropForeignKey("dbo.ArizaKayitDetaylari", "ArizaKayitId", "dbo.ArizaKayitlari");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "UserName" });
            DropIndex("dbo.AspNetUsers", new[] { "Email" });
            DropIndex("dbo.Kategoriler", new[] { "KategoriAdi" });
            DropIndex("dbo.ArizaKayitlari", new[] { "KategoriId" });
            DropIndex("dbo.ArizaKayitlari", new[] { "KullaniciId" });
            DropIndex("dbo.ArizaKayitDetaylari", new[] { "TeknisyenId" });
            DropIndex("dbo.ArizaKayitDetaylari", new[] { "OperatorId" });
            DropIndex("dbo.ArizaKayitDetaylari", new[] { "ArizaKayitId" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Kategoriler");
            DropTable("dbo.ArizaKayitlari");
            DropTable("dbo.ArizaKayitDetaylari");
        }
    }
}
