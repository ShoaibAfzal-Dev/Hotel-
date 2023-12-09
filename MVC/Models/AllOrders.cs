using System.ComponentModel.DataAnnotations;

namespace MVC.Models
{
    public class AllOrders
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int productwishid { get; set; }
        public string? Name { get; set; }
        public string? Category { get; set; }
        public string? Price { get; set; }
        public string? RoomNo { get; set; }
        public DateTime? StartingDate { get; set; }
        public DateTime? EndingDate { get; set; }
        public bool? Status { get; set; }
        public string? UserId { get; set; }
        public string? OrderVarification { get; set; }
    }
}
