namespace ProjetASPMVC1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Notes", "note_concours", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Notes", "note_concours", c => c.Double(nullable: false));
        }
    }
}
