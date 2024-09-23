using BookShop.Models;
using BookShop.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.ViewComponents
{
    public class CategoryViewComponent:ViewComponent
    {
        private readonly CategorySevice _categorySevice;
        public CategoryViewComponent( CategorySevice categorySevice) 
        {
            _categorySevice = categorySevice;
        }

        public IViewComponentResult Invoke()
        {
            return View("Category",_categorySevice.GetCategories());
        }
    }
}
