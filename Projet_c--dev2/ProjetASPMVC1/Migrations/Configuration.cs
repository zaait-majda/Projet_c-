namespace ProjetASPMVC1.Migrations
{
    using ProjetASPMVC1.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ProjetASPMVC1.Models.Projet_ContextBD>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ProjetASPMVC1.Models.Projet_ContextBD context)
        {

            var fil = new List<Filiere>();
            fil.Add(new Filiere { nom_fil = "Génie informatique" });
            fil.Add(new Filiere { nom_fil = "Génie GPMC" });
            fil.Add(new Filiere { nom_fil = "Génie Telecom" });
            fil.Add(new Filiere { nom_fil = "Génie industrielle" });
            base.Seed(context);
        }
    }
}
