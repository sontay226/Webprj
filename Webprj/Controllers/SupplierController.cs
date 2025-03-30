using Microsoft.AspNetCore.Mvc;
using Webprj.Models;

namespace Webprj.Controllers
{
    public class SupplierController : Controller
    {
        private readonly Test2WebContext _context;
        public SupplierController( Test2WebContext context ) => _context = context;
        public IActionResult SupplierView()
        {
            var data = _context.Suppliers.ToList();
            return View(data);
        }
        [HttpGet]
        public IActionResult DetailSupplier( int SupplierId )
        {
            var data = _context.Suppliers.Find(SupplierId);
            if (data != null) return View(data);
            return NotFound();
        }

        [HttpGet]
        public IActionResult DeleteSupplier( int SupplierId )
        {
            var data = _context.Suppliers.Find(SupplierId);
            if (data != null) return View(data);
            return NotFound();
        }

        [HttpPost]
        public IActionResult ConfirmDeleteSupplier( int SupplierId )
        {
            var data = _context.Suppliers.Find(SupplierId);
            if (data != null)
            {
                _context.Suppliers.Remove(data);
                _context.SaveChanges();
                return RedirectToAction("SupplierView");
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult EditSupplier( int SupplierId )
        {
            var data = _context.Suppliers.Find(SupplierId);
            if (data != null)
                return View(data);
            return NotFound();
        }

        [HttpPost]
        public IActionResult ConfirmEditSupplier( Supplier supplier)
        {
            var data = _context.Suppliers.Find(supplier.SupplierId);
            if (data != null)
            {
                data.ShortName = supplier.ShortName;
                data.FullName = supplier.FullName;
                data.Email = supplier.Email;
                data.PhoneNumber = supplier.PhoneNumber;
                data.Address = supplier.Address;
                _context.SaveChanges();
                return RedirectToAction("SupplierView");
            }
            return NotFound();
        }
    }
}
