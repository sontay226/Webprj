using Microsoft.AspNetCore.Mvc;
using Webprj.Models;

namespace Webprj.Controllers
{
    public class CategoryController : Controller
    {
        private readonly Test2WebContext _context;
        public CategoryController ( Test2WebContext context) => _context = context;
        public IActionResult CategoryView ()
        {
            var data = _context.Categories.ToList ();
            return View (data);
        }
        [HttpGet]
        public IActionResult DetailCategory ( int CategoryId )
        {
            var data = _context.Categories.Find (CategoryId);
            if (data != null) return View (data);
            return NotFound ();
        }
        // delete controll
        [HttpGet]
        public IActionResult DeleteCategory ( int CategoryId )
        {
            var data = _context.Categories.Find (CategoryId);
            if (data != null) return View (data);
            return NotFound ();
        }

        [HttpPost]
        public IActionResult ConfirmDeleteCategory( int CategoryId )
        {
            var data = _context.Categories.Find (CategoryId);
            if (data != null)
            {
                _context.Categories.Remove (data);
                _context.SaveChanges ();
                return RedirectToAction ("CategoryView");
            }
            return NotFound ();
        }
        // edit controll
        [HttpGet]
        public IActionResult EditCategory( int CategoryId )
        {
            var data = _context.Categories.Find (CategoryId);
            if (data != null) return View (data);
            return NotFound ();
        }
        [HttpPost]
        public IActionResult ConfirmEditCategory( Category category)
        {
            var data = _context.Categories.Find (category.CategoryId);
            if (data != null)
            {
                data.CategoryName = category.CategoryName;
                data.Description= category.Description;
                _context.SaveChanges ();
                return RedirectToAction ("CategoryView");
            }
            return NotFound ();
        }
    }
}
