using System.ComponentModel.DataAnnotations;

namespace Webprj.Models.ViewModel
{
    public class ForgotPasswordViewModel
    {
        [Required, EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
