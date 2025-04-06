using System.ComponentModel.DataAnnotations;

namespace Webprj.Models.ViewModel
{
    public class SignupViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập tên")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [Compare("Password" , ErrorMessage = "Mật khẩu không khớp")]
        public string ConfirmPassword { get; set; }
    }
}
