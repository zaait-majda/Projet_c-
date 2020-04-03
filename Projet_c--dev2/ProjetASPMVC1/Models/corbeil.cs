

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
    public partial class corbeil
    {
       
        [Key]
 
        public String CIN { set; get; }

        public String CNE { set; get; }
 

        public String prenom { set; get; }
   

        public String nom { set; get; }

        public String ville { set; get; }

        public String addresse { set; get; }

    
        public String GSM { set; get; }

        public String type_bac { set; get; }
      
        public String annee_bac { set; get; }
   
        public String note_bac { set; get; }
 
        public String mention_bac { set; get; }

        public String n_dossier { set; get; }
   
        public String nationnalite { set; get; }
       
        public String sexe { set; get; }
        public int cont_sup { set; get; }
        public int cont_ajout { set; get; }
    
        [DataType(DataType.Password)]
        public String password { set; get; }
        
        public String password_conf { set; get; }
  
        public String photo { set; get; }
       
        public String email { set; get; }
        public virtual bool EmailConfirmed { get; set; }
        public String statut { set; get; }
        public Boolean convocu { set; get; }
     
        public String niveau { set; get; }
    
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


    }
}