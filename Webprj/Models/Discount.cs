using System;
using System.Collections.Generic;

namespace Webprj.Models
{
    public partial class Discount
    {
        public Discount()
        {
            Products = new HashSet<Product>();
        }

        public int DiscountId { get; set; }
        public string DiscountCode { get; set; } = null!;
        public decimal? DiscountAmount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string DiscountType { get; set; } = null!;

        public virtual ICollection<Product> Products { get; set; }
    }
}
