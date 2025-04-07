using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Webprj.Models
{
    public partial class Customer : IdentityUser<int>
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
            CreatedAt = DateTime.Now;
        }
        public string CustomerName { get; set; } = null!;
        public string? ShippingAddress { get; set; }
        public string? BillingAddress { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
