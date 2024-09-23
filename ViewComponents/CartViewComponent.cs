using BookShop.Models;
using BookShop.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.ViewComponents
{
    public class CartViewComponent:ViewComponent
    {
        private readonly BookShopContext db;
        public CartViewComponent(BookShopContext context)
        {
            db = context;
        }

        public IViewComponentResult Invoke()
        {
            var customerId = HttpContext.Session.GetInt32("UserId");
            var result = db.Carts.Where(c => c.CustomerCustomerId == customerId)
                .Join(db.Products, c => c.ProductProductId, p => p.ProductId, (c, p) =>
                new CartVM()
                {
                    CartId=c.CartId,
                    CustomerId=c.CustomerCustomerId,
                    ProductId=p.ProductId,
                    Price=p.Price,
                    ProductName=p.ProductName,
                    Quantity=c.Quantity,
                    Image=p.Image,               
                });
            return View("Cart",result);
        }
    }
}
