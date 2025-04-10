using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webprj.Models;

namespace Webprj.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DiscountController : Controller
    {
        private readonly Test2WebContext _context;
        public DiscountController ( Test2WebContext context ) => _context = context;
        public IActionResult DiscountView ()
        {
            var data = _context.Discounts.ToList ();
            return View (data);
        }
        [HttpGet]
        public IActionResult DetailDiscount ( int DiscountId )
        {
            var data = _context.Discounts.Find (DiscountId);
            if (data != null) return View (data);
            return NotFound ();
        }
        // delete controll
        [HttpGet]
        public IActionResult DeleteDiscount( int DiscountId )
        {
            var data = _context.Discounts.Find (DiscountId);
            if (data != null) return View (data);
            return NotFound ();
        }

        [HttpPost]
        public IActionResult ConfirmDeleteDiscount ( int DiscountId )
        {
            var data = _context.Discounts.Find (DiscountId);
            if (data != null)
            {
                _context.Discounts.Remove (data);
                _context.SaveChanges ();
                return RedirectToAction ("DiscountView");
            }
            return NotFound ();
        }
        // edit controll
        [HttpGet]
        public IActionResult EditDiscount ( int DiscountId )
        {
            var data = _context.Discounts.Find (DiscountId);
            if (data != null) return View (data);
            return NotFound ();
        }
        [HttpPost]
        public IActionResult ConfirmEditDiscount ( Discount discount)
        {
            var data = _context.Discounts.Find (discount.DiscountId);
            if (data != null)
            {
                data.DiscountCode = discount.DiscountCode;
                data.DiscountAmount = discount.DiscountAmount;
                data.StartDate = discount.StartDate;
                data.DiscountType = discount.DiscountType;
                data.EndDate = discount.EndDate;
                _context.SaveChanges ();
                return RedirectToAction ("DiscountView");
            }
            return NotFound ();
        }
        [HttpGet]
        public IActionResult CreateDiscount()
        {
            return View(new Discount());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmCreateDiscount( Discount discount)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    _context.Discounts.Add(discount);
                    _context.SaveChanges();
                    return RedirectToAction("DiscountView");
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
                return View("CreateDiscount" , discount);
            }
            return View("CreateDiscount" , discount);
        }
    }
}
