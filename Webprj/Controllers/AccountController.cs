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
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<Customer> userManager ,IEmailSender emailSender )
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword( [Bind(include: "Email")] ForgotPasswordViewModel emailVerification )
        {
            Console.WriteLine($"Thuc thi forget password post method: {emailVerification.Email}");

            var user = await _userManager.FindByEmailAsync(emailVerification.Email);
            Console.WriteLine($"Thuc thi forget password post method: {emailVerification.Email}");
            Console.WriteLine($"user information: {user.Email}");

            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            return RedirectToAction(nameof(ForgotPasswordConfirmation));

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            Console.WriteLine($"new token for reset password{token}");
            var callbackUrl = Url.Action(
                nameof(ResetPassword) , "Account" ,
                new { token , email = user.Email } ,
                protocol: Request.Scheme);

            var htmlMsg = $@"
                <p>Chào {user.CustomerName},</p>
                <p>Bạn vừa yêu cầu đặt lại mật khẩu. Nhấp vào <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>đây</a> để tiếp tục.</p>
                <p>Nếu không phải bạn, vui lòng bỏ qua email này.</p>";

            Console.WriteLine("Chuan bi gui email");
            await _emailSender.SendEmailAsync(emailVerification.Email , "Reset mật khẩu" , htmlMsg);
            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation() => View();

        [HttpGet]
        public IActionResult ResetPassword( string token , string email )
        {
            if (token == null || email == null)
                return RedirectToAction(nameof(ForgotPassword));

            return View(new ResetPasswordViewModel { Token = token , Email = email });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword( ResetPasswordViewModel vm )
        {
            if (!ModelState.IsValid) return View(vm);

            var user = await _userManager.FindByEmailAsync(vm.Email);
            Console.WriteLine($"user {user?.Email}");
            Console.WriteLine($"{vm.Password} {vm.ConfirmPassword}");

            if (user == null)
                return RedirectToAction(nameof(ResetPasswordConfirmation));

            var result = await _userManager.ResetPasswordAsync(user , vm.Token , vm.Password);
            Console.WriteLine($"result: {result.Succeeded}");

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            foreach (var err in result.Errors)
            {
                Console.WriteLine($"Error: {err.Description}");
                ModelState.AddModelError("" , err.Description);
            }

            if (result.Errors.Any(e => e.Code.Contains("InvalidToken") || e.Description.Contains("expired")))
            {
                return View("~/Views/Account/ConfirmEmailFailed.cshtml");
            }
            return View(vm);
        }


        [HttpGet]
        public IActionResult ResetPasswordConfirmation() => View();
    }
}