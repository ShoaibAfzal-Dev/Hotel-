using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class Registeration
    {
        [Required]
        public string Name { get; set; }
        [Required(ErrorMessage = "Phone Number Required!")]
       /* [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                   ErrorMessage = "Entered phone format is not valid.")]*/
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email {  get;  set; } = null!;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password and confirm password do not match")]
        public string ConfirmPassword { get; set; }
    }
}
