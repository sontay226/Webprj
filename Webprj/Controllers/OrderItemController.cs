using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Webprj.Models;
using Webprj.Models.ViewModel;

namespace Webprj.Controllers
{

    public class OrderItemController : Controller
    {
        private readonly Test2WebContext _context;
        public OrderItemController( Test2WebContext context ) => _context = context;
        [Authorize(Roles = "Admin")]
        public IActionResult OrderItemView()
        {
            var data = _context.OrderItems.ToList();
            return View(data);
        }
        [Authorize(Roles = "Admin")]

        [HttpGet]
        public IActionResult DetailOrderItem( int OrderItemId )
        {
            var data = _context.OrderItems.Find(OrderItemId);
            if (data != null) return View(data);
            return NotFound();
        }
        [Authorize(Roles = "Admin")]

        [HttpGet]
        public IActionResult DeleteOrderItem( int OrderItemId )
        {
            var data = _context.OrderItems.Find(OrderItemId);
            if (data != null) return View(data);
            return NotFound();
        }
        [Authorize(Roles = "Admin")]

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
        [Authorize(Roles = "Admin")]

        [HttpGet]
        public IActionResult EditOrderItem( int OrderItemId )
        {
            var data = _context.OrderItems.Find(OrderItemId);
            if (data != null)
                return View(data);
            return NotFound();
        }
        [Authorize(Roles = "Admin")]

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
        public async Task<IActionResult> ConfirmCreateOrderItem( int id , int quantity )
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null || quantity <= 0)
                {
                    ModelState.AddModelError("" , "Sản phẩm không hợp lệ hoặc số lượng không hợp lệ.");
                    return RedirectToAction("ProductInformation" , "Product" , new { id = id });
                }

                var cart = await GetOrCreateCartAsync();
                var existingItem = await _context.OrderItems
                    .FirstOrDefaultAsync(oi => oi.OrderId == cart.OrderId && oi.ProductId == id);

                if (existingItem != null)
                {
                    existingItem.ProductNumber += quantity;
                    existingItem.TotalCost = product.Price * existingItem.ProductNumber;
                }
                else
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = cart.OrderId ,
                        ProductId = id ,
                        ProductNumber = quantity ,
                        TotalCost = product.Price * quantity
                    };
                    _context.OrderItems.Add(orderItem);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction("Cart");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                ModelState.AddModelError("" , "Đã xảy ra lỗi khi lưu dữ liệu.");
                return RedirectToAction("ProductInformation" , "Product" , new { id = id });
            }
        }
        [Authorize]
        public async Task<IActionResult> Cart()
        {
            var cart = await GetOrCreateCartAsync();
            var vm = new CartVm
            {
                OrderID = cart.OrderId ,
                Items = cart.OrderItems!
                    .Select(oi => new CartItemVm
                    {
                        OrderItemID = oi.OrderItemId ,
                        ProductID = oi.ProductId ,
                        ProductName = oi.Product!.Name ,
                        ImageUrl = oi.Product.ImageUrl ,
                        Quantity = oi.ProductNumber ,
                        UnitPrice = oi.Product.Price ,
                        TotalCost = (decimal)oi.TotalCost
                    })
                    .ToList()
            };
            return View(vm);
        }

        private async Task<Order?> GetOrCreateCartAsync ()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var cart = await _context.Orders.Include(o => o.OrderItems!).ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync( o => o.CustomerId == userId && o.Status == "Pending");
            if ( cart == null)
            {
                cart = new Order
                {
                    CustomerId = userId ,
                    OrderDate = DateTime.Now ,
                    Status = "Pending" ,
                    TotalAmount = 0 ,
                };
                _context.Orders.Add(cart);
                await _context.SaveChangesAsync();
            }
            return cart;
        }
        // xử lý số lượng tồn
        [HttpPost, Authorize]
        public async Task<IActionResult> UpdateQuantity( int orderItemId , int quantity )
        {
            var item = await _context.OrderItems
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.OrderItemId == orderItemId);

            if (item == null)
            {
                TempData["Error"] = "Không tìm thấy sản phẩm trong giỏ hàng.";
                return RedirectToAction("Cart");
            }

            if (item.Product == null)
            {
                TempData["Error"] = $"Sản phẩm không tồn tại trong hệ thống (ProductId = {item.ProductId}).";
                return RedirectToAction("Cart");
            }

            if (quantity <= 0)
            {
                TempData["Error"] = "Số lượng không hợp lệ.";
                return RedirectToAction("Cart");
            }

            item.ProductNumber = quantity;
            item.TotalCost = item.Product.Price * quantity;

            await _context.SaveChangesAsync();
            return RedirectToAction("Cart");
        }

        // xử lý xóa sản phầm
        [HttpPost, Authorize]
        public async Task<IActionResult> RemoveItem( int orderItemId )
        {
            var item = await _context.OrderItems.FindAsync(orderItemId);
            if (item != null)
            {
                _context.OrderItems.Remove(item);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Cart");
        }
        [HttpPost, Authorize]
        public async Task<IActionResult> Checkout( CheckoutViewModel vm )
        {
            var order = await _context.Orders.Include(o => o.OrderItems!).ThenInclude(oi => oi.Product).FirstOrDefaultAsync(o => o.OrderId == vm.OrderID && o.Status == "Pending");
            if (order == null) return NotFound();
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (order.CustomerId != userId) return Forbid();
            foreach (var item in order.OrderItems!)
                if (item.Product!.StockQuantity < item.ProductNumber)
                    ModelState.AddModelError("" , $"'{item.Product.Name}' chỉ còn {item.Product.StockQuantity}.");

            if (!ModelState.IsValid)
            {
                vm.PayMethods = GetPayMethods();
                return View(vm);
            }

            // cập nhật order
            order.Status = "Processing";
            order.OrderDate = DateTime.Now;
            order.TotalAmount = (decimal) order.OrderItems.Sum(oi => oi.TotalCost);
            foreach (var item in order.OrderItems!)
                item.Product!.StockQuantity -= item.ProductNumber;

            // tạo payment
            var payment = new Payment
            {
                OrderId = order.OrderId ,
                TransactionDate = DateTime.Now ,
                PayMethod = vm.SelectedPayMethod
            };
            _context.Payments.Add(payment);

            await _context.SaveChangesAsync();
            return RedirectToAction("OrderConfirmation" , new { paymentId = payment.PaymentId });
        }

        [HttpGet, Authorize]
        public async Task<IActionResult> Checkout( int orderId )
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems!)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId && o.Status == "Pending");

            if (order == null)
                return RedirectToAction("Cart");

            var vm = new CheckoutViewModel
            {
                OrderID = order.OrderId ,
                Items = order.OrderItems!
                    .Select(oi => new CartItemVm
                    {
                        OrderItemID = oi.OrderItemId ,
                        ProductID = oi.ProductId ,
                        ProductName = oi.Product!.Name ,
                        ImageUrl = oi.Product.ImageUrl ,
                        Quantity = oi.ProductNumber ,
                        UnitPrice = oi.Product.Price ,
                        TotalCost = (decimal) oi.TotalCost
                    })
                    .ToList() ,
                Subtotal = (decimal) order.OrderItems.Sum(oi => oi.TotalCost) ,
                PayMethods = GetPayMethods()
            };
            return View(vm);
        }

        private IEnumerable<SelectListItem> GetPayMethods() => new List<SelectListItem> { 
            new SelectListItem("Thanh Toán Khi Nhận Hàng(COD)" , "cash"),
            new SelectListItem("Chuyển khoản ngân hàng (Bank)" , "bank_transfer"),
            new SelectListItem("Ví Momo" , "momo"),
            new SelectListItem("Thẻ tín dụng" , "credit_card")
        };

        [HttpGet, Authorize]
        public async Task<IActionResult> OrderConfirmation( int paymentId )
        {
            var payment = await _context.Payments
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.PaymentId == paymentId);
            if (payment == null) return NotFound();

            return View(payment);
        }
    }
}
