using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Webprj.Models;
using Webprj.Models.ViewModel;

namespace Webprj.Controllers
{
    public class CustomerController : Controller
    {
        private readonly Test2WebContext _context;
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;
        public CustomerController( Test2WebContext context , UserManager<Customer> um , SignInManager<Customer> sm)
        {
            _userManager = um;
            _signInManager = sm;
            _context = context;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CustomerView ()
        {
            var data = await _context.Customers.ToListAsync ();
            return View ( data );
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> DetailCustomer ( int id)
        {
            var data = await _context.Customers.FindAsync (id);
            if (data != null) return  View (data);
            return NotFound ();
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> DeleteCustomer ( int id )
        {
            var data = await _context.Customers.FindAsync (id);
            if (data != null) return View (data);
            return NotFound ();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ConfirmDeleteCustomer ( int id)
        {
            var data = await _context.Customers.FindAsync (id);
            if (data != null)
            {
                _context.Customers.Remove (data);
                await _context.SaveChangesAsync ();
                return RedirectToAction ("CustomerView");
            }
            return NotFound ();
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> EditCustomer ( int id)
        {
            var data = await _context.Customers.FindAsync (id);
            if (data != null) return View (data);
            return NotFound ();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmEditCustomer( Customer customer)
        {
            if ( !ModelState.IsValid )
            {
                return View("EditCustomer" , customer);
            }
            var data = _context.Customers.Find (customer.Id);
            if (data != null)
            {
                data.CustomerName = customer.CustomerName;
                data.Email = customer.Email;
                data.PhoneNumber = customer.PhoneNumber;
                data.ShippingAddress = customer.ShippingAddress;
                data.BillingAddress = customer.BillingAddress;
                data.PasswordHash = customer.PasswordHash;
                data.CreatedAt = customer.CreatedAt;
                _context.Customers.Update (data);
                await _context.SaveChangesAsync ();
                return RedirectToAction ("CustomerView");
            }
            return NotFound ();
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
        public async Task<IActionResult> ConfirmCreateCustomer( Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    customer.CreatedAt = DateTime.Now;
                    _context.Customers.Add(customer);
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
            return View( new SigninViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signin( SigninViewModel model )
        {
            if ( !ModelState.IsValid ) return View( model );
            var ans = await _signInManager.PasswordSignInAsync(
                    model.Email ,
                    model.Password,
                    isPersistent: false,
                    lockoutOnFailure : false
                );
            return RedirectToAction("Index" , "Home");
        }
        // đăng ký *almost done
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Signup() => View(new SignupViewModel());
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup(SignupViewModel model )
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
                CreatedAt = DateTime.UtcNow
            };
            var result = await _userManager.CreateAsync( user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var e in result.Errors)
                    ModelState.AddModelError("" , e.Description);
                return View(model);
            }
            await _userManager.AddToRoleAsync(user , "User");
            await _signInManager.SignInAsync(user , isPersistent: false);
            return RedirectToAction("Index" , "Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index" , "Home");
        }
        [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> FindCustomer( string name )
        {
            if (string.IsNullOrEmpty(name))
            {
                var all = await _context.Customers.ToListAsync();
                return View("CustomerView" , all);
            }
            var matched = await _context.Customers.Where(p => EF.Functions.Like(p.CustomerName, $"%{name}%")).ToListAsync();
            return View("CustomerView" , matched);
        }
    }
}
    
