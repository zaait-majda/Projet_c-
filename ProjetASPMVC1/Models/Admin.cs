using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjetASPMVC1.Models
{
    public partial class Admin
    {
        [Key]
        public int login { set; get; }

        public int password { set; get; }
    }
}