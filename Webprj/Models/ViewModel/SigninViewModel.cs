using System.ComponentModel.DataAnnotations;

namespace Webprj.Models.ViewModel
{
    public class SigninViewModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
