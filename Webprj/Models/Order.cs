using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;

namespace Webprj.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
            Payments = new HashSet<Payment>();
            Shipments = new HashSet<Shipment>();
        }

        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime? OrderDate { get; set; }
        public string? ShippingAddress { get; set; }
        public string Status { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public string? PaymentMethod { get; set; }
        [ValidateNever]
        public virtual Customer? Customer { get; set; } = null!;
        public virtual ICollection<OrderItem>? OrderItems { get; set; }
        public virtual ICollection<Payment>? Payments { get; set; }
        public virtual ICollection<Shipment>? Shipments { get; set; }
    }
}
