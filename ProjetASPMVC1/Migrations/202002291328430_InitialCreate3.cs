namespace ProjetASPMVC1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Candidats", "photo", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Candidats", "photo", c => c.Binary());
        }
    }
}
