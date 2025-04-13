using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webprj.Models;
using Microsoft.EntityFrameworkCore;

namespace Webprj.Controllers
{

    public class ProductController : Controller
    {
        private readonly Test2WebContext _context;
        public ProductController (Test2WebContext context) => _context = context;
        [Authorize(Roles = "Admin")]
        public IActionResult ProductView()
        {
            var data = _context.Products.ToList();
            return View (data);
        }
        // adding controll 
        // detail controll 
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult DetailProduct(int ProductId )
        {
            var data = _context.Products.Find (ProductId);
            if (data != null) return View (data);
            return NotFound ();
        }
        [Authorize(Roles = "Admin")]

        // delete controll
        [HttpGet]
        public IActionResult DeleteProduct ( int ProductId )
        {
            var data = _context.Products.Find (ProductId);
            if (data != null) return View (data);
            return NotFound ();
        }
        [Authorize(Roles = "Admin")]

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
        [Authorize(Roles = "Admin")]

        // edit controll
        [HttpGet]
        public IActionResult EditProduct ( int ProductId )
        {
            var data = _context.Products.Find (ProductId);
            if ( data != null) return View (data);
            return NotFound ();
        }
        [Authorize(Roles = "Admin")]

        [HttpPost]
        public IActionResult ConfirmEditProduct ( Product product )
        {
            var data = _context.Products.Find (product.ProductId);
            if ( data != null )
            {
                data.Name = product.Name;
                data.Description = product.Description;
                data.Price = product.Price;
                data.StockQuantity = product.StockQuantity;
                data.ImageUrl = product.ImageUrl;
                data.Certificate = product.Certificate;
                data.TechnicalSpecifications = product.TechnicalSpecifications;
                data.CreatedAt = product.CreatedAt;
                data.UpdatedAt = DateTime.Now;
                data.CategoryId = product.CategoryId;
                data.SupplierId = product.SupplierId;

                _context.Products.Update (data);
                _context.SaveChanges ();
                return RedirectToAction ("ProductView");
            }
            return NotFound (); 
        }
        [Authorize(Roles = "Admin")]

        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View(new Product());
        }
        [Authorize(Roles = "Admin")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmCreateProduct( Product product , IFormFile imageFile )
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var fileName = Path.GetFileName(imageFile.FileName);
                        var filePath = Path.Combine("wwwroot/images" , fileName);

                        using (var stream = new FileStream(filePath , FileMode.Create))
                        {
                            imageFile.CopyTo(stream);
                        }
                        product.ImageUrl = $"/images/{fileName}";
                    }
                    product.CreatedAt = DateTime.Now;
                    product.UpdatedAt = DateTime.Now;
                    _context.Products.Add(product);
                    _context.SaveChanges();
                    return RedirectToAction("ProductView");
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
                return View("CreateProduct" , product);
            }
            return View("CreateProduct" , product);
        }
        // thông tin chi tiết của từng sản phẩm 
        public async Task<IActionResult> ProductInformation ( int id )
        {
            var product = await _context.Products.FindAsync(id);
            if ( product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        // tìm kiếm sản phẩm
        [HttpGet]
        public async Task<IActionResult> Search( string query )
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                ViewBag.Query = "";
                return View("SearchResults" , new List<Product>());
            }

            ViewBag.Query = query;
            var matched = await _context.Products
                .Where(p => EF.Functions.Like(p.Name , $"%{query}%"))
                .ToListAsync();

            return View("SearchResults" , matched);
        }
    }

}
