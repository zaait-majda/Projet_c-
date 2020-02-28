using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
namespace ProjetASPMVC1.Models
{
    public partial class Candidat
    {
        [Key]
        public String CIN { set; get; }
        public String CNE { set; get; }
        public String prenom { set; get; }
        public String sexe { set; get; }
        public String password { set; get; }
        public byte[] photo { set; get; }
        public String statut { set; get; }
        public String niveau { set; get; }
        public Nullable<System.DateTime> date_naiss { set; get; }
        [ForeignKey("Filiere")]
        public Nullable<int> id_fil { set; get; }

        public virtual Filiere Filiere { set; get; }
        [ForeignKey("Notes")]
        public Nullable<int> id_note { set; get; }

        public virtual Notes Notes { set; get; }
        [ForeignKey("Diplome")]
        public Nullable<int> id_diplome { set; get; }
       
        public virtual Diplome Diplome { set; get; }


    }
}