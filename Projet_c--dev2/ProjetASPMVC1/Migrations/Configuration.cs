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
            fil.Add(new Filiere { nom_fil = "G�nie informatique" });
            fil.Add(new Filiere { nom_fil = "G�nie GPMC" });
            fil.Add(new Filiere { nom_fil = "G�nie Telecom" });
            fil.Add(new Filiere { nom_fil = "G�nie industrielle" });
            base.Seed(context);
        }
    }
}
