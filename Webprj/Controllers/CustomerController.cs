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
    }
}
