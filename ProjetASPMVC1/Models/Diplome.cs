using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
namespace ProjetASPMVC1.Models
{
    public partial class Diplome
    {
        public Diplome()
        {
            this.Candidats = new HashSet<Candidat>();
        }
        [Key]
        public int id_diplome { set; get; }
        public String nom_diplome { set; get; }

        public String etablissement { set; get; }
        public virtual ICollection<Candidat> Candidats { set; get; }
    }
}