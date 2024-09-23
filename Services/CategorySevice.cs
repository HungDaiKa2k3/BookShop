using BookShop.Models;

namespace BookShop.Services
{
    public class CategorySevice
    {
        private readonly BookShopContext _context;
        public CategorySevice(BookShopContext context)
        {
            _context = context;
        }
        public List<Category> GetCategories() 
        {
            return _context.Categories.ToList();
        }
    }
}
