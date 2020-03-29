namespace ProjetASPMVC1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notes", "notemath", c => c.Double(nullable: false));
            AddColumn("dbo.Notes", "notespec", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notes", "notespec");
            DropColumn("dbo.Notes", "notemath");
        }
    }
}
