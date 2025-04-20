using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Webprj.Models;

namespace Webprj.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PaymentController : Controller
    {
        private readonly Test2WebContext _context;
        public PaymentController( Test2WebContext context ) => _context = context;
        public IActionResult PaymentView()
        {
            var data = _context.Payments.ToList();
            return View(data);
        }
        [HttpGet]
        public IActionResult DetailPayment( int PaymentId )
        {
            var data = _context.Payments.Find(PaymentId);
            if (data != null) return View(data);
            return NotFound();
        }

        [HttpGet]
        public IActionResult DeletePayment( int PaymentId )
        {
            var data = _context.Payments.Find(PaymentId);
            if (data != null) return View(data);
            return NotFound();
        }

        [HttpPost]
        public IActionResult ConfirmDeletePayment( int PaymentId )
        {
            var data = _context.Payments.Find(PaymentId);
            if (data != null)
            {
                _context.Payments.Remove(data);
                _context.SaveChanges();
                return RedirectToAction("PaymentView");
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult EditPayment( int PaymentId )
        {
            var data = _context.Payments.Find(PaymentId);
            if (data != null)
                return View(data);
            return NotFound();
        }

        [HttpPost]
        public IActionResult ConfirmEditPayment( Payment payment )
        {
            var data = _context.Payments.Find(payment.PaymentId);
            if (data != null)
            {
                data.TransactionDate = payment.TransactionDate;
                data.PayMethod = payment.PayMethod;
                _context.SaveChanges();
                return RedirectToAction("PaymentView");
            }
            return NotFound();
        }
        [HttpGet]
        public IActionResult CreatePayment()
        {
            return View(new Payment());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmCreatePayment( Payment payment )
        {
            if (ModelState.IsValid)
            {
                try
                {
                    payment.TransactionDate = DateTime.Now;
                    _context.Payments.Add(payment);
                    _context.SaveChanges();
                    return RedirectToAction("PaymentView");
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
                return View("CreatePayment" , payment);
            }
            return View("CreatePayment" , payment);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> FindPayment( string id )
        {
            if (int.TryParse(id , out int paymentId))
            {
                var matched = await _context.Payments
                    .Where(p => p.PaymentId == paymentId)
                    .ToListAsync();
                return View("PaymentView" , matched);
            }
            else
            {
                var all = await _context.Payments.ToListAsync();
                return View("PaymentView" , all);
            }
        }
    }
}