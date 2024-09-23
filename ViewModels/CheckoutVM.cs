using BookShop.Models;

namespace BookShop.ViewModels
{
    public class CheckoutVM
    {
        public List<CartVM>? CartVMs { get; set; } = new List<CartVM>();
        public Payment? Payment { get; set; } = new Payment(); 
        public Shipment? Shipment { get; set; } = new Shipment(); 
        public Customer? Customer { get; set; } = new Customer(); 
    }

}
