namespace ProjetASPMVC1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingConvocuAttribut : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Candidats", "convocu", c => c.Boolean(nullable: false,defaultValue:false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Candidats", "convocu");
        }
    }
}
