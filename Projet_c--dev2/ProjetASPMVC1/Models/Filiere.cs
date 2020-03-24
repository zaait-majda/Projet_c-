
namespace ProjetASPMVC1.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Filiere
    {
        public Filiere()
        {
            this.Candidats = new HashSet<Candidat>();
        }
        [Key]
        public int id_fil { get; set; }
        [Required]
        public string nom_fil { get; set; }


        public virtual ICollection<Candidat> Candidats { get; set; }
    }
}
