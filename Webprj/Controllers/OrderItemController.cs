using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webprj.Models;

namespace Webprj.Controllers
{
    [Authorize(Roles = "Admin")]

    public class OrderItemController : Controller
    {
        private readonly Test2WebContext _context;
        public OrderItemController( Test2WebContext context ) => _context = context;
        public IActionResult OrderItemView()
        {
            var data = _context.OrderItems.ToList();
            return View(data);
        }

        [HttpGet]
        public IActionResult DetailOrderItem( int OrderItemId )
        {
            var data = _context.OrderItems.Find(OrderItemId);
            if (data != null) return View(data);
            return NotFound();
        }

        [HttpGet]
        public IActionResult DeleteOrderItem( int OrderItemId )
        {
            var data = _context.OrderItems.Find(OrderItemId);
            if (data != null) return View(data);
            return NotFound();
        }

        [HttpPost]
        public IActionResult ConfirmDeleteOrderItem( int OrderItemId )
        {
            var data = _context.OrderItems.Find(OrderItemId);
            if (data != null)
            {
                _context.OrderItems.Remove(data);
                _context.SaveChanges();
                return RedirectToAction("OrderItemView");
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult EditOrderItem( int OrderItemId )
        {
            var data = _context.OrderItems.Find(OrderItemId);
            if (data != null)
                return View(data);
            return NotFound();
        }

        [HttpPost]
        public IActionResult ConfirmEditOrderItem( OrderItem orderItem )
        {
            var data = _context.OrderItems.Find(orderItem.OrderItemId);
            if (data != null)
            {
                data.OrderId = orderItem.OrderId;
                data.ProductId = orderItem.ProductId;
                data.ProductNumber = orderItem.ProductNumber;
                data.TotalCost = orderItem.TotalCost;
                _context.SaveChanges();
                return RedirectToAction("OrderItemView");
            }
            return NotFound();
        }
        [HttpGet]
        public IActionResult CreateOrderItem()
        {
            return View(new OrderItem());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmCreateOrderItem( OrderItem orderItem)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    _context.OrderItems.Add(orderItem);
                    _context.SaveChanges();
                    return RedirectToAction("OrderItemView");
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
                return View("CreateOrderItem" , orderItem);
            }
            return View("CreateOrderItem" , orderItem);
        }
    }
}
