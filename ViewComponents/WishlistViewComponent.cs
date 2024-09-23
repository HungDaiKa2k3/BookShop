using BookShop.Models;
using BookShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.ViewComponents
{
    public class WishlistViewComponent:ViewComponent
    {
        private readonly BookShopContext db;
        public WishlistViewComponent(BookShopContext context)
        {
            db = context;
        }
        public IViewComponentResult Invoke()
        {
            int userId = HttpContext.Session.GetInt32("UserId")??0;

            var kq = from w in db.Wishlists
                     join c in db.Customers on w.CustomerCustomerId equals c.CustomerId
                     join p in db.Products on w.ProductProductId equals p.ProductId
                     where c.CustomerId == userId
                     orderby w.WishlistId descending
                     select new WishlistVM()
                     {
                         ProductId = p.ProductId,
                         ProductName=p.ProductName,
                         Image=p.Image,
                         Price = p.Price,
                     };

            var kq2 = from w in db.Wishlists
                     join p in db.Products on w.ProductProductId equals p.ProductId
                     where w.CustomerCustomerId == userId
                     select new WishlistVM()
                     {
                         ProductName = p.ProductName,
                         Image = p.Image,
                         Price = p.Price,
                     };

            var kq3 = db.Wishlists.Include(w => w.CustomerCustomer)
                .Include(w => w.ProductProduct)
                .Where(w=>w.CustomerCustomerId==userId)
                .Select(w => new WishlistVM()
                {
                    ProductName = w.ProductProduct.ProductName,
                    Image = w.ProductProduct.Image,
                    Price = w.ProductProduct.Price,               
                });

            var kq4 = db.Wishlists.Join(db.Customers, w => w.CustomerCustomerId, c => c.CustomerId, (w, c) => new { w, c }).Where(ww=>ww.w.CustomerCustomerId==userId)
                .Join(db.Products, w2 => w2.w.ProductProductId, p => p.ProductId, (w2, p) => new WishlistVM()
                {
                    ProductName= p.ProductName,
                    Image = p.Image,
                    Price = p.Price,
                });

            return View("WishlistVC",kq);
        }
    }
}
