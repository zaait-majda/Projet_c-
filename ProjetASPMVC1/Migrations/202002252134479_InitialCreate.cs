namespace ProjetASPMVC1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Admins",
                c => new
                    {
                        login = c.Int(nullable: false, identity: true),
                        password = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.login);
            
            CreateTable(
                "dbo.Diplomes",
                c => new
                    {
                        id_diplome = c.Int(nullable: false, identity: true),
                        nom_diplome = c.String(),
                        etablissement = c.String(),
                    })
                .PrimaryKey(t => t.id_diplome);
            
            CreateTable(
                "dbo.Candidats",
                c => new
                    {
                        CIN = c.String(nullable: false, maxLength: 128),
                        CNE = c.String(),
                        prenom = c.String(),
                        sexe = c.String(),
                        password = c.String(),
                        photo = c.Binary(),
                        statut = c.String(),
                        date_naiss = c.DateTime(),
                        id_fil = c.Int(),
                        id_note = c.Int(),
                        id_diplome = c.Int(),
                    })
                .PrimaryKey(t => t.CIN)
                .ForeignKey("dbo.Diplomes", t => t.id_diplome)
                .ForeignKey("dbo.Filieres", t => t.id_fil)
                .ForeignKey("dbo.Notes", t => t.id_note)
                .Index(t => t.id_diplome)
                .Index(t => t.id_fil)
                .Index(t => t.id_note);
            
            CreateTable(
                "dbo.Filieres",
                c => new
                    {
                        id_fil = c.Int(nullable: false, identity: true),
                        nom_fil = c.String(),
                    })
                .PrimaryKey(t => t.id_fil);
            
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        id_note = c.Int(nullable: false, identity: true),
                        note_diplome = c.Double(nullable: false),
                        note_concours = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.id_note);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Candidats", "id_note", "dbo.Notes");
            DropForeignKey("dbo.Candidats", "id_fil", "dbo.Filieres");
            DropForeignKey("dbo.Candidats", "id_diplome", "dbo.Diplomes");
            DropIndex("dbo.Candidats", new[] { "id_note" });
            DropIndex("dbo.Candidats", new[] { "id_fil" });
            DropIndex("dbo.Candidats", new[] { "id_diplome" });
            DropTable("dbo.Notes");
            DropTable("dbo.Filieres");
            DropTable("dbo.Candidats");
            DropTable("dbo.Diplomes");
            DropTable("dbo.Admins");
        }
    }
}
