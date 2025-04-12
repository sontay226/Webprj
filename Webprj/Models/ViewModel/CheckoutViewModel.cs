using Microsoft.AspNetCore.Mvc.Rendering;
using Webprj.Models.ViewModel;
namespace Webprj.Models.ViewModel
{
    public class CheckoutViewModel
    {
        public int OrderID { get; set; }
        public List<CartItemVm> Items { get; set; } = new();
        public decimal Subtotal { get; set; }
        public string SelectedPayMethod { get; set; } = "COD";
        public IEnumerable<SelectListItem> PayMethods { get; set; } = new List<SelectListItem>
        {
            new SelectListItem("Thanh toán khi nhận hàng (COD)", "COD"),
            new SelectListItem("Chuyển khoản ngân hàng (ATM)", "ATM"),
            new SelectListItem("Ví Momo", "Momo")
        };
    }
}
