namespace ProjetASPMVC1.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using ProjetASPMVC1.Models;
    internal sealed class Configuration : DbMigrationsConfiguration<ProjetASPMVC1.Models.Projet_ContextBD>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ProjetASPMVC1.Models.Projet_ContextBD context)
        {

            var fil = new List<Filiere>();
            fil.Add(new Filiere { nom_fil = "Génie Informatique" });
            fil.Add(new Filiere { nom_fil = "Génie Procédés et Matériaux Céramiques" });
            fil.Add(new Filiere { nom_fil = "Génie Télécommunications et Réseaux" });
            fil.Add(new Filiere { nom_fil = "Génie Industriel" });
            context.Filieres.AddRange(fil);
            base.Seed(context);
        }
    }
}
