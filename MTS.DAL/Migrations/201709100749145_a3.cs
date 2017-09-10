namespace MTS.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ArizaKayitDetaylari", "OperatorIslemZamani", c => c.DateTime());
            AlterColumn("dbo.ArizaKayitDetaylari", "BitisZamani", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ArizaKayitDetaylari", "BitisZamani", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ArizaKayitDetaylari", "OperatorIslemZamani", c => c.DateTime(nullable: false));
        }
    }
}
