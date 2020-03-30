namespace ProjetASPMVC1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Notes", "math", c => c.Double());
            AddColumn("dbo.Notes", "specialite", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notes", "specialite");
            DropColumn("dbo.Notes", "math");
        }
    }
}
