namespace MTS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ArizaKayitDetaylari", "Fiyat", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.ArizaKayitlari", "Puan", c => c.Byte());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ArizaKayitlari", "Puan", c => c.Byte(nullable: false));
            AlterColumn("dbo.ArizaKayitDetaylari", "Fiyat", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
