using System;
using System.Collections.Generic;

namespace Webprj.Models
{
    public partial class OrderItem
    {
        public int OrderItemId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int ProductNumber { get; set; }
        public decimal? TotalCost { get; set; }

        public virtual Order? Order { get; set; } = null!;
        public virtual Product? Product { get; set; } = null!;
    }
}
