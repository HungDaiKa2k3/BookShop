namespace BookShop.ViewModels
{
    public class CartVM
    {
        public int CartId { get; set; }

        public int? Quantity { get; set; }

        public int? CustomerId { get; set; }

        public int? ProductId { get; set; }
        public string? ProductName { get; set; }
        public int? Price {  get; set; }
        public string? Image { get; set; }
        public int? Total => Price * Quantity;

    }
}
