using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shoppng_Tutorial.Repository;

namespace Shoppng_Tutorial.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataContext _dataContext;

        public ProductController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Search(string searchTerm)
        {
            var products = await _dataContext.Products
                .Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
                .ToListAsync();

            ViewBag.Keyword = searchTerm;

            return View(products);
        }


        public async Task<IActionResult> Details(int Id)
        {
            if (Id == null) return RedirectToAction("Index");
            var productById = _dataContext.Products.Where(p => p.Id == Id).FirstOrDefault();
            var relatedProducts = await _dataContext.Products
                    .Include(p => p.Category)
                    .Include(p => p.Brand)
                    .Where(p => p.CategoryId == productById.CategoryId && p.Id != productById.Id)
                    .Take(4)
                    .ToListAsync();

            ViewBag.RelatedProducts = relatedProducts;

            return View(productById);
        }
    }
}
