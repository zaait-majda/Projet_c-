using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjetASPMVC1.Models
{
    public partial class Filiere
    {
        public Filiere()
        {
            this.Candidats = new HashSet<Candidat>();
        }
        [Key]
        public int id_fil { set; get; }
        public String nom_fil { set; get; }
        public virtual ICollection<Candidat> Candidats { set; get; }
    }
}