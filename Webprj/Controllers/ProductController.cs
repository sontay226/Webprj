﻿using Microsoft.AspNetCore.Mvc;
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
        // adding controll 
        // detail controll 
        [HttpGet]
        public IActionResult DetailProduct(int ProductId )
        {
            var data = _context.Products.Find (ProductId);
            if (data != null) return View (data);
            return NotFound ();
        }
        // delete controll
        [HttpGet]
        public IActionResult DeleteProduct ( int ProductId )
        {
            var data = _context.Products.Find (ProductId);
            if (data != null) return View (data);
            return NotFound ();
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
        // edit controll
        [HttpGet]
        public IActionResult EditProduct ( int ProductId )
        {
            var data = _context.Products.Find (ProductId);
            if ( data != null) return View (data);
            return NotFound ();
        }
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

        [HttpGet]
        public IActionResult CreateProduct()
        {
            return View(new Product());
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public IActionResult ConfirmCreateProduct( Product product )
        {
            if (ModelState.IsValid)
            {
                try
                {
                    product.CreatedAt = DateTime.Now;
                    product.UpdatedAt = DateTime.Now;
                    _context.Products.Add(product);
                    _context.SaveChanges();
                    return RedirectToAction("ProductView");
                }
                catch (Exception ex)
                {
                    Console.WriteLine (ex.ToString ());
                    ModelState.AddModelError("" , "Đã xảy ra lỗi khi lưu dữ liệu.");
                }
            }
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }
            return View("CreateProduct" , product);
        }
    }
}
