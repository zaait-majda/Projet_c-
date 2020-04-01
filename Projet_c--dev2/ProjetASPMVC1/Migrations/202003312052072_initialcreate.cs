namespace ProjetASPMVC1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialcreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.corbeils",
                c => new
                    {
                        CIN = c.String(nullable: false, maxLength: 128),
                        CNE = c.String(),
                        prenom = c.String(),
                        nom = c.String(),
                        ville = c.String(),
                        addresse = c.String(),
                        GSM = c.String(),
                        type_bac = c.String(),
                        annee_bac = c.String(),
                        note_bac = c.String(),
                        mention_bac = c.String(),
                        n_dossier = c.String(),
                        nationnalite = c.String(),
                        sexe = c.String(),
                        cont_sup = c.Int(nullable: false),
                        cont_ajout = c.Int(nullable: false),
                        password = c.String(),
                        password_conf = c.String(),
                        photo = c.String(),
                        email = c.String(),
                        EmailConfirmed = c.Boolean(nullable: false),
                        statut = c.String(),
                        convocu = c.Boolean(nullable: false),
                        niveau = c.String(),
                        date_naiss = c.DateTime(nullable: false),
                        id_fil = c.Int(nullable: false),
                        id_note = c.Int(nullable: false),
                        id_diplome = c.Int(nullable: false),
                        nom_dip = c.String(),
                    })
                .PrimaryKey(t => t.CIN)
                .ForeignKey("dbo.Diplomes", t => t.id_diplome, cascadeDelete: true)
                .ForeignKey("dbo.Filieres", t => t.id_fil, cascadeDelete: true)
                .ForeignKey("dbo.Notes", t => t.id_note, cascadeDelete: true)
                .Index(t => t.id_fil)
                .Index(t => t.id_note)
                .Index(t => t.id_diplome);
            
 
            AddColumn("dbo.Candidats", "nom_dip", c => c.String());
            AddColumn("dbo.Notes", "notemath", c => c.Double());
            AddColumn("dbo.Notes", "notespec", c => c.Double());
            DropColumn("dbo.Notes", "math");
            DropColumn("dbo.Notes", "specialite");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Notes", "specialite", c => c.Double());
            AddColumn("dbo.Notes", "math", c => c.Double());
            DropForeignKey("dbo.corbeils", "id_note", "dbo.Notes");
            DropForeignKey("dbo.corbeils", "id_fil", "dbo.Filieres");
            DropForeignKey("dbo.corbeils", "id_diplome", "dbo.Diplomes");
            DropIndex("dbo.corbeils", new[] { "id_diplome" });
            DropIndex("dbo.corbeils", new[] { "id_note" });
            DropIndex("dbo.corbeils", new[] { "id_fil" });
            DropColumn("dbo.Notes", "notespec");
            DropColumn("dbo.Notes", "notemath");
            DropColumn("dbo.Candidats", "nom_dip");
            DropTable("dbo.corbeils");
        }
    }
}
