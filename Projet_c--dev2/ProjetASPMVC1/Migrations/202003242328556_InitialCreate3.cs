namespace ProjetASPMVC1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BoitMessages", "Nom", c => c.String());
            AlterColumn("dbo.BoitMessages", "message", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BoitMessages", "message", c => c.Int(nullable: false));
            AlterColumn("dbo.BoitMessages", "Nom", c => c.Int(nullable: false));
        }
    }
}
