using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;

namespace Webprj.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderItems = new HashSet<OrderItem>();
            Discounts = new HashSet<Discount>();
        }

        public int ProductId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public string? ImageUrl { get; set; }
        public string? Certificate { get; set; }
        public string? TechnicalSpecifications { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [ValidateNever]
        public virtual Category? Category { get; set; } = null!;
        [ValidateNever]
        public virtual Supplier? Supplier { get; set; } = null!;
        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public virtual ICollection<Discount> Discounts { get; set; }
    }
}