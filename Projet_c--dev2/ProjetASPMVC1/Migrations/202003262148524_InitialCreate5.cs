namespace ProjetASPMVC1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BoitMessages",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Nom = c.String(),
                        message = c.String(),
                        vue = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BoitMessages");
        }
    }
}
