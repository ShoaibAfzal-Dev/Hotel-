using System.ComponentModel.DataAnnotations;
using WebApi.Models;

namespace MVC.Models
{
    public class MyView
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string RoomNo { get; set; }
        public List<IFormFile> imageFiles { get; set; }
    }
}
