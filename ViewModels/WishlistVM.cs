namespace BookShop.ViewModels
{
    public class WishlistVM
    {
        public int WishlistId { get; set; }
        public int ProductId {  get; set; }
        public string? ProductName { get; set; }

        public int? Price { get; set; }

        public string? Image { get; set; }
    }
}
