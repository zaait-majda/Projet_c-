﻿using System;
using System.Collections.Generic;
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
        public double note_diplome { set; get; }
        public double? note_concours { set; get; }
        public virtual ICollection<Candidat> Candidats { set; get; }
    }
}