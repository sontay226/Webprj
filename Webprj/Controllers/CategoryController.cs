using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webprj.Models;

namespace Webprj.Controllers
{
    [Authorize(Roles = "Admin")]
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
        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View(new Category());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmCreateCategory( Category category )
        {
            if (ModelState.IsValid)
            {
                try
                {

                    _context.Categories.Add(category);
                    _context.SaveChanges();
                    return RedirectToAction("CategoryView");
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
                return View("CreateCategory" , category );
            }
            return View("CreateCategory" , category);
        }
    }
}
