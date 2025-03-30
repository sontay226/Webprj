using Microsoft.AspNetCore.Mvc;
using Webprj.Models;

namespace Webprj.Controllers
{
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
    }
}
