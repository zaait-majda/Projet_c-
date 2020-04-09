using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ProjetASPMVC1.Models
{
    public partial class Candidat
    {
        public Candidat() { cont_sup = 0; cont_ajout = 0; statut = "inscrit"; EmailConfirmed = false; }
        [Key]
        [Required]
        public String CIN { set; get; }
        [Required]
        public String CNE { set; get; }
        [Required]

        public String prenom { set; get; }
        [Required]

        public String nom { set; get; }

        [Required]
        public String ville { set; get; }

        [Required]
        public String addresse { set; get; }

        [Required]
        public String tel { set; get; }
        [Required]
        public String GSM { set; get; }
        [Required]
        public String type_bac { set; get; }
        [Required]
        public String annee_bac { set; get; }
        [Required]
        public String note_bac { set; get; }
        [Required]
        public String mention_bac { set; get; }

        public String n_dossier { set; get; }
        [Required]
        public String nationnalite { set; get; }

        [Required(ErrorMessage = "Choose your sexe ")]
        public String sexe { set; get; }
        public int cont_sup { set; get; }
        public int cont_ajout { set; get; }
        [Required]
        [DataType(DataType.Password)]
        public String password { set; get; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("password", ErrorMessage = "les champs ne sont pas compatibles")]
        public String password_conf { set; get; }
        [Required]
        public String photo { set; get; }
        [Required]
        public String email { set; get; }
        public virtual bool EmailConfirmed { get; set; }
        public String statut { set; get; }
        public Boolean convocu { set; get; }
        [Required]
        public String niveau { set; get; }
        [Required]
        public System.DateTime date_naiss { set; get; }
        [ForeignKey("Filiere")]
        public int id_fil { set; get; }

        public virtual Filiere Filiere { set; get; }
        [ForeignKey("Notes")]
        public int id_note { set; get; }

        public virtual Notes Notes { set; get; }
        [ForeignKey("Diplome")]
        public int id_diplome { set; get; }

        public string nom_dip { set; get; }

        public virtual Diplome Diplome { set; get; }
        [Required]
        public String redoublant { set; get; }

    }
}