using Microsoft.AspNetCore.Mvc;
using Webprj.Models;

namespace Webprj.Controllers
{
    public class ProductController : Controller
    {
        private readonly Test2WebContext _context;
        public ProductController (Test2WebContext context) => _context = context;
        public IActionResult ProductView()
        {
            var data = _context.Products.ToList();
            return View (data);
        }
        [HttpGet]
        public IActionResult DeleteProduct ( int ProductId)
        {
            var data = _context.Products.Find (ProductId);
            if ( data != null) return View (data);
            return NotFound();
        }
        [HttpPost]
        public IActionResult ConfirmDeleteProduct( int ProductId) 
        {
            var data = _context.Products.Find (ProductId);
            if ( data != null )
            {
                _context.Products.Remove(data);
                _context.SaveChanges();
                return RedirectToAction ("ProductView");
            }
            return NotFound();
        }
    }
}
