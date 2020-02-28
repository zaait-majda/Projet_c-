namespace ProjetASPMVC1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Candidats", "niveau", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Candidats", "niveau");
        }
    }
}
