namespace Webprj.Models.ViewModel
{
    public class CartItemVm
    {
        public int OrderItemID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalCost { get; set; }
    }

    public class CartVm
    {
        public int OrderID { get; set; }
        public List<CartItemVm> Items { get; set; } = new();
        public decimal Subtotal => Items.Sum(i => i.TotalCost);
    }

}
