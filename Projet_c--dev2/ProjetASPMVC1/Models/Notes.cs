using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjetASPMVC1.Models
{
    public class Notes
    {
        public Notes()
        {
            this.Candidats = new HashSet<Candidat>();
        }
        [Key]
        public int id_note { set; get; }
        [Required]
        public double s1 { set; get; }
        [Required]
        public double s2 { set; get; }
        [Required]
        public double s3 { set; get; }
        [Required]
        public double s4 { set; get; }
       
       
        public double? s5 { set; get; }
       
      
        public double? s6 { set; get; }

        public double? note_concours { set; get; }
        public double? math { set; get; }
        public double? specialite { set; get; }
        public virtual ICollection<Candidat> Candidats { set; get; }
    }
}