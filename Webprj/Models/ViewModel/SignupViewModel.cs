using System.ComponentModel.DataAnnotations;

namespace Webprj.Models.ViewModel
{
    public class SignupViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = null;

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null;

        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu")]
        [DataType(DataType.Password)]
        [Compare("Password" , ErrorMessage = "Mật khẩu không khớp")]
        public string ConfirmPassword { get; set; } = null;
        [Required(ErrorMessage = "Vui lòng nhập tên")]
        public string CustomerName { get; set; }
        [Phone]
        [Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Địa chỉ giao hàng")]
        public string? ShippingAddress { get; set; }

        [Display(Name = "Địa chỉ thanh toán")]
        public string? BillingAddress { get; set; }
    }
}
