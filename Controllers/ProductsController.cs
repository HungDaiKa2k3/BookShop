using BookShop.Models;
using BookShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.Extensions;

namespace BookShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly BookShopContext db;
        public ProductsController(BookShopContext context)
        {
            db = context;
        }
        public IActionResult Index(int? page,int? category)
        {
            int pageNumber = page ?? 1;
            int pageSize = 6;
            var products = db.Products.AsQueryable();
            if (category.HasValue) 
            {
                products=products.Where(p=>p.CategoryCategoNavigation.CategoryId == category.Value);
            }
            var result=products.Select(p=>new ProductVM()
            {
                ProductId = p.ProductId,
                Price = p.Price,
                ProductName = p.ProductName,
                Image=p.Image,
                Description=p.Description,
            }).ToList();
            var pagedResult=result.ToPagedList(pageNumber, pageSize);

            if (HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("ListProducts", pagedResult);
            }

            return View(pagedResult);
        }

        public IActionResult QuickView(int ProductId)
        {
            var result=db.Products.Where(p=>p.ProductId == ProductId).Select(p=>new ProductVM()
            {
                ProductId=p.ProductId,
                ProductName=p.ProductName,
                Image=p.Image,
                Description=p.Description,
                Price=p.Price,
            }).FirstOrDefault();
            ViewBag.QuickView=result;
            return Json(result);
        }

        [HttpGet("/viewdetail")]
        public IActionResult ViewDetail(int productId)
        {
            var result = db.Products.FirstOrDefault(p => p.ProductId == productId);
            return View(result);
        }

    }
}
