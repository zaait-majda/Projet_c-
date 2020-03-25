namespace ProjetASPMVC1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BoitMessages", "vue", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BoitMessages", "vue");
        }
    }
}
