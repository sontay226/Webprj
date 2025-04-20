using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using Webprj.Models;
using Webprj.Models.ViewModel;
using Webprj.Models.ViewModels;
using Webprj.Services;

namespace Webprj.Controllers
{
    public class CustomerController : Controller
    {
        private readonly Test2WebContext _context;
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;
        private readonly IEmailSender _emailSender;

        public CustomerController( Test2WebContext context , UserManager<Customer> um , SignInManager<Customer> sm , IEmailSender emailSender )
        {
            _userManager = um;
            _signInManager = sm;
            _context = context;
            _emailSender = emailSender;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CustomerView()
        {
            var data = await _context.Users.ToListAsync();
            return View(data);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> DetailCustomer( int id )
        {
            var data = await _context.Users.FindAsync(id);
            if (data != null) return View(data);
            return NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> DeleteCustomer( int id )
        {
            var data = await _context.Users.FindAsync(id);
            if (data != null) return View(data);
            return NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ConfirmDeleteCustomer( int id )
        {
            var data = await _context.Users.FindAsync(id);
            if (data != null)
            {
                _context.Users.Remove(data);
                await _context.SaveChangesAsync();
                return RedirectToAction("CustomerView");
            }
            return NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditCustomer( int id )
        {
            var data = await _context.Users.FindAsync(id);
            if (data != null) return View(data);
            return NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmEditCustomer( Customer customer )
        {
            if (!ModelState.IsValid)
            {
                return View("EditCustomer" , customer);
            }
            var data = _context.Users.Find(customer.Id);
            if (data != null)
            {
                data.CustomerName = customer.CustomerName;
                data.Email = customer.Email;
                data.PhoneNumber = customer.PhoneNumber;
                data.ShippingAddress = customer.ShippingAddress;
                data.BillingAddress = customer.BillingAddress;
                data.PasswordHash = customer.PasswordHash;
                data.CreatedAt = customer.CreatedAt;
                _context.Users.Update(data);
                await _context.SaveChangesAsync();
                return RedirectToAction("CustomerView");
            }
            return NotFound();
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateCustomer()
        {
            return View(new Customer());
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmCreateCustomer( Customer customer )
        {
            if (ModelState.IsValid)
            {
                try
                {
                    customer.CreatedAt = DateTime.Now;
                    _context.Users.Add(customer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("CustomerView");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    ModelState.AddModelError("" , "Đã xảy ra lỗi khi lưu dữ liệu.");
                }
            }

            if (!ModelState.IsValid)
            {
                var allErrors = ModelState
                    .Where(kvp => kvp.Value.Errors.Any())
                    .Select(kvp => new
                    {
                        Field = kvp.Key ,
                        Errors = kvp.Value.Errors.Select(e => e.ErrorMessage)
                    })
                    .ToList();
                return View("CreateCustomer" , customer);
            }
            return View("CreateCustomer" , customer);
        }
        // đăng nhập
        [HttpGet]
        public IActionResult Signin()
        {
            return View(new SigninViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signin( SigninViewModel model )
        {
            var user = await _userManager.FindByEmailAsync( model.Email );
            if ( user == null)
            {
                ModelState.AddModelError(string.Empty , "Sai email hoặc mật khẩu");
                return View(model);
            }
            if ( !user.EmailConfirmed )
            {
                ModelState.AddModelError(string.Empty , "Vui lòng xác nhận email trước khi đăng nhập!");
                return View(model);
            }
            if (!ModelState.IsValid) return View(model);
            var result = await _signInManager.PasswordSignInAsync(
                model.Email ,
                model.Password ,
                isPersistent: false ,
                lockoutOnFailure: false
            );

            if (result.Succeeded)
            {
                return RedirectToAction("Index" , "Home");
            }

            ModelState.AddModelError(string.Empty , "Sai email hoặc mật khẩu.");
            return View(model);

        }
        // đăng ký *almost done
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Signup() => View(new SignupViewModel());
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup( SignupViewModel model )
        {
            if (!ModelState.IsValid) { return View(model); }
            var user = new Customer
            {
                UserName = model.Email ,
                Email = model.Email ,
                CustomerName = model.CustomerName ,
                PhoneNumber = model.PhoneNumber ,
                ShippingAddress = model.ShippingAddress ,
                BillingAddress = model.BillingAddress ,
                CreatedAt = DateTime.UtcNow ,
                EmailConfirmed = false
            };
            var result = await _userManager.CreateAsync(user , model.Password);
            if (!result.Succeeded)
            {
                foreach (var e in result.Errors) ModelState.AddModelError("" , e.Description);
                return View(model);
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Action(nameof(ConfirmEmail) , "Customer" ,
                new { email = user.Email , token = WebUtility.UrlEncode(token) } ,
                protocol: Request.Scheme);

            var htmlMessage = $@"
            <p>Chào {user.CustomerName},</p>
            <p>Vui lòng xác nhận tài khoản bằng cách nhấn vào liên kết sau:</p>
            <a href=""{callbackUrl}"">Xác nhận Email</a>";

            await _emailSender.SendEmailAsync(user.Email , "Xác nhận Email" , htmlMessage);

            await _userManager.AddToRoleAsync(user , "User");
            return View("SignupConfirmation" , new SignupConfirmationViewModel { Email = user.Email });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index" , "Home");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> FindCustomer( string name )
        {
            if (string.IsNullOrEmpty(name))
            {
                var all = await _context.Users.ToListAsync();
                return View("CustomerView" , all);
            }
            var matched = await _context.Users.Where(p => EF.Functions.Like(p.CustomerName , $"%{name}%")).ToListAsync();
            return View("CustomerView" , matched);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail ( string email , string token )
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(token)) return View("Error");
            var user = await _userManager.FindByEmailAsync(email);
            if ( user == null) return View("Error");
            var decodedToken = WebUtility.UrlDecode(token);
            var result = await _userManager.ConfirmEmailAsync(user , decodedToken);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user , isPersistent: false);
                return View("~/Views/Account/ConfirmEmail.cshtml");
            }

            return View("~/Views/Account/ConfirmEmailFailed.cshtml");
        }
    }
}
