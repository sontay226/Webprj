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
        [HttpGet]
        public IActionResult CreateSupplier()
        {
            return View(new Supplier());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmCreateSupplier( Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Suppliers.Add(supplier);
                    _context.SaveChanges();
                    return RedirectToAction("SupplierView");
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
                return View("CreateSupplier" , supplier);
            }
            return View("CreateSupplier" , supplier);
        }
    }
}
