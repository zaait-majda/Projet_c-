using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;

namespace ProjetASPMVC1.Models
{
    public class Projet_ContextBD : DbContext
    {
        public Projet_ContextBD() : base("BD_Projet_csharp")
        {
        }
        public DbSet<Candidat> Candidats { get; set; }
        public DbSet<Filiere> Filieres { get; set; }
        public DbSet<Notes> Notes { get; set; }
        public DbSet<Diplome> Diplomes { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<BoitMessage> message { get; set; }
        public DbSet<corbeil> corbeils { get; set; }
        
    }
}