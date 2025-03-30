using Microsoft.AspNetCore.Mvc;
using Webprj.Models;

namespace Webprj.Controllers
{
    public class ShipmentController : Controller
    {
        private readonly Test2WebContext _context;
        public ShipmentController( Test2WebContext context ) => _context = context;
        public IActionResult ShipmentView()
        {
            var data = _context.Shipments.ToList();
            return View(data);
        }
        [HttpGet]
        public IActionResult DetailShipment( int ShipmentId )
        {
            var data = _context.Shipments.Find(ShipmentId);
            if (data != null) return View(data);
            return NotFound();
        }

        [HttpGet]
        public IActionResult DeleteShipment( int ShipmentId )
        {
            var data = _context.Shipments.Find(ShipmentId);
            if (data != null) return View(data);
            return NotFound();
        }

        [HttpPost]
        public IActionResult ConfirmDeleteShipment( int ShipmentId )
        {
            var data = _context.Shipments.Find(ShipmentId);
            if (data != null)
            {
                _context.Shipments.Remove(data);
                _context.SaveChanges();
                return RedirectToAction("ShipmentView");
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult EditShipment( int ShipmentId )
        {
            var data = _context.Shipments.Find(ShipmentId);
            if (data != null)
                return View(data);
            return NotFound();
        }

        [HttpPost]
        public IActionResult ConfirmEditShipment( Shipment shipment)
        {
            var data = _context.Shipments.Find(shipment.ShipmentId);
            if (data != null)
            {
                data.ShippingDate = shipment.ShippingDate;
                data.PurchaseDate = shipment.PurchaseDate;
                data.ShipperName = shipment.ShipperName;
                _context.SaveChanges();
                return RedirectToAction("ShipmentView");
            }
            return NotFound();
        }
    }
}
