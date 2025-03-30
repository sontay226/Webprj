using Microsoft.AspNetCore.Mvc;
using Webprj.Models;

namespace Webprj.Controllers
{
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
        public IActionResult DetailPayment( int PaymentId)
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
        public IActionResult ConfirmDeletePayment( int PaymentId)
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
        public IActionResult EditPayment( int PaymentId)
        {
            var data = _context.Payments.Find(PaymentId);
            if (data != null)
                return View(data);
            return NotFound();
        }

        [HttpPost]
        public IActionResult ConfirmEditPayment( Payment payment)
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
    }
}
