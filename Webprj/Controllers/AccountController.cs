using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Webprj.Models;
using Webprj.Models.ViewModel;
using Webprj.Services;

namespace Webprj.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController(
            UserManager<Customer> userManager ,
            SignInManager<Customer> signInManager ,
            IEmailSender emailSender )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        // ===== Forgot Password =====

        [HttpGet]
        public IActionResult ForgotPassword()
            => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword( ForgotPasswordViewModel vm )
        {
            if (!ModelState.IsValid)
                return View(vm);

            var user = await _userManager.FindByEmailAsync(vm.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Không tiết lộ user tồn tại hay không
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action(
                nameof(ResetPassword) ,
                "Account" ,
                new { token , email = vm.Email } ,
                protocol: Request.Scheme);

            var htmlMessage = $@"
                <p>Chào {user.CustomerName},</p>
                <p>Bạn vừa yêu cầu đặt lại mật khẩu. Vui lòng nhấp vào liên kết bên dưới:</p>
                <p><a href='{HtmlEncoder.Default.Encode(callbackUrl!)}'>Đặt lại mật khẩu</a></p>
                <p>Nếu bạn không yêu cầu, hãy bỏ qua email này.</p>";

            await _emailSender.SendEmailAsync(
                vm.Email ,
                "Reset mật khẩu trên Webprj" ,
                htmlMessage);

            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
            => View();

        // ===== Reset Password =====

        [HttpGet]
        public IActionResult ResetPassword( string token , string email )
        {
            if (token == null || email == null)
                return RedirectToAction(nameof(ForgotPassword));

            var vm = new ResetPasswordViewModel { Token = token , Email = email };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword( ResetPasswordViewModel vm )
        {
            if (!ModelState.IsValid)
                return View(vm);

            var user = await _userManager.FindByEmailAsync(vm.Email);
            if (user == null)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            var result = await _userManager.ResetPasswordAsync(user , vm.Token , vm.Password);
            if (result.Succeeded)
                return RedirectToAction(nameof(ResetPasswordConfirmation));

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty , error.Description);

            return View(vm);
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
            => View();
    }
}
