using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webprj.Models;

namespace Webprj.Controllers
{
    public class CustomerController : Controller
    {
        private readonly Test2WebContext _context;
        public CustomerController ( Test2WebContext context ) => _context = context;
        public IActionResult CustomerView ()
        {
            var data = _context.Customers.ToList ();
            return View ( data );
        }
        [HttpGet]
        public IActionResult DetailCustomer ( int CustomerId)
        {
            var data = _context.Customers.Find (CustomerId);
            if (data != null) return View (data);
            return NotFound ();
        }
        // delete controll
        [HttpGet]
        public IActionResult DeleteCustomer ( int CustomerId )
        {
            var data = _context.Customers.Find (CustomerId);
            if (data != null) return View (data);
            return NotFound ();
        }

        [HttpPost]
        public IActionResult ConfirmDeleteCustomer ( int CustomerId )
        {
            var data = _context.Customers.Find (CustomerId);
            if (data != null)
            {
                _context.Customers.Remove (data);
                _context.SaveChanges ();
                return RedirectToAction ("CustomerView");
            }
            return NotFound ();
        }
        // edit controll
        [HttpGet]
        public IActionResult EditCustomer ( int CustomerId)
        {
            var data = _context.Customers.Find (CustomerId);
            if (data != null) return View (data);
            return NotFound ();
        }
        [HttpPost]
        public IActionResult ConfirmEditCustomer( Customer customer)
        {
            var data = _context.Customers.Find (customer.CustomerId);
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
                _context.SaveChanges ();
                return RedirectToAction ("CustomerView");
            }
            return NotFound ();
        }
        [HttpGet]
        public IActionResult CreateCustomer()
        {
            return View(new Customer());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmCreateCustomer( Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    customer.CreatedAt = DateTime.Now;
                    _context.Customers.Add(customer);
                    _context.SaveChanges();
                    return RedirectToAction("CustomerView");
                }
                catch (Exception ex)
                {
                    // Ghi log lỗi
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
    }
}
