using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webprj.Models;

namespace Webprj.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly Test2WebContext _context;
        public OrderController( Test2WebContext context ) => _context = context;
        public IActionResult OrderView()
        {
            var data = _context.Orders.ToList();
            return View(data);
        }
        [HttpGet]
        public IActionResult DetailOrder( int OrderId )
        {
            var data = _context.Orders.Find(OrderId);
            if (data != null) return View(data);
            return NotFound();
        }
        // delete controll
        [HttpGet]
        public IActionResult DeleteOrder( int OrderId )
        {
            var data = _context.Orders.Find(OrderId);
            if (data != null) return View(data);
            return NotFound();
        }

        [HttpPost]
        public IActionResult ConfirmDeleteOrder( int OrderId )
        {
            var data = _context.Orders.Find(OrderId);
            if (data != null)
            {
                _context.Orders.Remove(data);
                _context.SaveChanges();
                return RedirectToAction("OrderView");
            }
            return NotFound();
        }
        // edit controll
        [HttpGet]
        public IActionResult EditOrder( int OrderId )
        {
            var data = _context.Orders.Find(OrderId);
            if (data != null) return View(data);
            return NotFound();
        }
        [HttpPost]
        public IActionResult ConfirmEditOrder( Order order )
        {
            var data = _context.Orders.Find(order.OrderId);
            if (data != null)
            {
                data.CustomerId = order.CustomerId;
                data.OrderDate = order.OrderDate;
                data.ShippingAddress = order.ShippingAddress;
                data.Status = order.Status;
                data.TotalAmount = order.TotalAmount;
                data.PaymentMethod = order.PaymentMethod;
                _context.SaveChanges();
                return RedirectToAction("OrderView");
            }
            return NotFound();
        }
        [HttpGet]
        public IActionResult CreateOrder()
        {
            return View(new Order());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmCreateOrder( Order order )
        {
            if (ModelState.IsValid)
            {
                try
                {
                    order.OrderDate = DateTime.Now;
                    _context.Orders.Add(order);
                    _context.SaveChanges();
                    return RedirectToAction("OrderView");
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
                return View("CreateOrder" , order);
            }
            return View("Create" , order);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> FindOrder( string id )
        {
            if (int.TryParse(id , out int orderId))
            {
                var matched = await _context.Orders
                    .Where(p => p.OrderId == orderId)
                    .ToListAsync();
                return View("OrderView" , matched);
            }
            else
            {
                var all = await _context.Orders.ToListAsync();
                return View("OrderView" , all);
            }
        }
    }
}