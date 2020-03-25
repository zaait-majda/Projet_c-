using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace ProjetASPMVC1.Models
{
    public class BoitMessage
    {
        [Key]

        public int id { set; get; }
        public string Nom { set; get; }
        public string message { set; get; }
        public int  vue{set; get;}
    }
}