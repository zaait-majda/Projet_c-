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
                        id = c.Int(nullable: false, identity: true),
                        Login = c.String(nullable: false),
                        password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Candidats",
                c => new
                    {
                        CIN = c.String(nullable: false, maxLength: 128),
                        CNE = c.String(nullable: false),
                        prenom = c.String(nullable: false),
                        nom = c.String(nullable: false),
                        ville = c.String(nullable: false),
                        addresse = c.String(nullable: false),
                        tel = c.String(nullable: false),
                        GSM = c.String(nullable: false),
                        type_bac = c.String(nullable: false),
                        annee_bac = c.String(nullable: false),
                        note_bac = c.String(nullable: false),
                        mention_bac = c.String(nullable: false),
                        n_dossier = c.String(),
                        nationnalite = c.String(nullable: false),
                        sexe = c.String(nullable: false),
                        cont_sup = c.Int(nullable: false),
                        cont_ajout = c.Int(nullable: false),
                        password = c.String(nullable: false),
                        password_conf = c.String(nullable: false),
                        photo = c.String(nullable: false),
                        email = c.String(nullable: false),
                        EmailConfirmed = c.Boolean(nullable: false),
                        statut = c.String(),
                        niveau = c.String(nullable: false),
                        date_naiss = c.DateTime(nullable: false),
                        id_fil = c.Int(nullable: false),
                        id_note = c.Int(nullable: false),
                        id_diplome = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CIN)
                .ForeignKey("dbo.Diplomes", t => t.id_diplome, cascadeDelete: true)
                .ForeignKey("dbo.Filieres", t => t.id_fil, cascadeDelete: true)
                .ForeignKey("dbo.Notes", t => t.id_note, cascadeDelete: true)
                .Index(t => t.id_fil)
                .Index(t => t.id_note)
                .Index(t => t.id_diplome);
            
            CreateTable(
                "dbo.Diplomes",
                c => new
                    {
                        id_diplome = c.Int(nullable: false, identity: true),
                        nom_diplome = c.String(nullable: false),
                        ville_diplome = c.String(nullable: false),
                        etablissement = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id_diplome);
            
            CreateTable(
                "dbo.Filieres",
                c => new
                    {
                        id_fil = c.Int(nullable: false, identity: true),
                        nom_fil = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.id_fil);
            
            CreateTable(
                "dbo.Notes",
                c => new
                    {
                        id_note = c.Int(nullable: false, identity: true),
                        s1 = c.Double(nullable: false),
                        s2 = c.Double(nullable: false),
                        s3 = c.Double(nullable: false),
                        s4 = c.Double(nullable: false),
                        s5 = c.Double(),
                        s6 = c.Double(),
                        note_concours = c.Double(),
                    })
                .PrimaryKey(t => t.id_note);
            
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
            DropForeignKey("dbo.Candidats", "id_note", "dbo.Notes");
            DropForeignKey("dbo.Candidats", "id_fil", "dbo.Filieres");
            DropForeignKey("dbo.Candidats", "id_diplome", "dbo.Diplomes");
            DropIndex("dbo.Candidats", new[] { "id_diplome" });
            DropIndex("dbo.Candidats", new[] { "id_note" });
            DropIndex("dbo.Candidats", new[] { "id_fil" });
            DropTable("dbo.BoitMessages");
            DropTable("dbo.Notes");
            DropTable("dbo.Filieres");
            DropTable("dbo.Diplomes");
            DropTable("dbo.Candidats");
            DropTable("dbo.Admins");
        }
    }
}
