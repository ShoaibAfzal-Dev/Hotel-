using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebApi.Models
{
    public class Role
    {
        [Required]
        [DisplayName("Role")]
        public string RoleName { get; set; }
    }
}
