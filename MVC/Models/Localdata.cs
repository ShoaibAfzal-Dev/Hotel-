using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class Localdata
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int productwishid { get; set; }

        public string productname { get; set; } 
    }
}
