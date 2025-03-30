using Microsoft.AspNetCore.Mvc;
using Webprj.Models;

namespace Webprj.Controllers
{
    public class OrderController : Controller
    {
        private readonly Test2WebContext _context;
        public OrderController ( Test2WebContext context ) => _context = context;
        public IActionResult OrderView ()
        {
            var data = _context.Orders.ToList ();
            return View (data);
        }
        [HttpGet]
        public IActionResult DetailOrder( int OrderId)
        {
            var data = _context.Orders.Find (OrderId);
            if (data != null) return View (data);
            return NotFound ();
        }
        // delete controll
        [HttpGet]
        public IActionResult DeleteOrder( int OrderId )
        {
            var data = _context.Orders.Find (OrderId);
            if (data != null) return View (data);
            return NotFound ();
        }

        [HttpPost]
        public IActionResult ConfirmDeleteOrder( int OrderId )
        {
            var data = _context.Orders.Find (OrderId);
            if (data != null)
            {
                _context.Orders.Remove (data);
                _context.SaveChanges ();
                return RedirectToAction ("OrderView");
            }
            return NotFound ();
        }
        // edit controll
        [HttpGet]
        public IActionResult EditOrder( int OrderId )
        {
            var data = _context.Orders.Find (OrderId);
            if (data != null) return View (data);
            return NotFound ();
        }
        [HttpPost]
        public IActionResult ConfirmEditOrder( Order order)
        {
            var data = _context.Orders.Find (order.OrderId);
            if (data != null)
            {
                data.CustomerId= order.CustomerId;
                data.OrderDate = order.OrderDate;
                data.ShippingAddress = order.ShippingAddress;
                data.Status = order.Status;
                data.TotalAmount = order.TotalAmount;
                data.PaymentMethod = order.PaymentMethod;
                _context.SaveChanges ();
                return RedirectToAction ("OrderView");
            }
            return NotFound ();
        }
    }
}
