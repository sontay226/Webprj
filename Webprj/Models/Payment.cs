using System;
using System.Collections.Generic;

namespace Webprj.Models
{
    public partial class Payment
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string PayMethod { get; set; } = null!;

        public virtual Order? Order { get; set; } = null!;
    }
}
