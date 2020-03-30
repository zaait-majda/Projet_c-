namespace ProjetASPMVC1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notes", "notemath", c => c.Double());
            AddColumn("dbo.Notes", "notespec", c => c.Double());
            DropColumn("dbo.Notes", "math");
            DropColumn("dbo.Notes", "specialite");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notes", "specialite", c => c.Double());
            AddColumn("dbo.Notes", "math", c => c.Double());
            DropColumn("dbo.Notes", "notespec");
            DropColumn("dbo.Notes", "notemath");
        }
    }
}
