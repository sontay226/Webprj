using System;
using System.Collections.Generic;

namespace Webprj.Models
{
    public partial class Shipment
    {
        public int ShipmentId { get; set; }
        public int OrderId { get; set; }
        public DateTime? ShippingDate { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public string? ShipperName { get; set; }

        public virtual Order Order { get; set; } = null!;
    }
}
