
namespace ProjetASPMVC1.Models
{
  
    using System.ComponentModel.DataAnnotations;

    public partial class Admin
    {
        [Required]
        public int id { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string password { get; set; }

    }
}
