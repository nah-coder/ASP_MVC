using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shoppng_Tutorial.Models;
using Shoppng_Tutorial.Models.ViewModel;
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
            var productById = _dataContext.Products.Include(p=>p.Ratings).Where(p => p.Id == Id).FirstOrDefault();
            var relatedProducts = await _dataContext.Products
                    .Include(p => p.Category)
                    .Include(p => p.Brand)
                    .Where(p => p.CategoryId == productById.CategoryId && p.Id != productById.Id)
                    .Take(4)
                    .ToListAsync();

            ViewBag.RelatedProducts = relatedProducts;

            var ratings = await _dataContext.Rating
                .Where(r => r.ProductId == Id)
                .OrderByDescending(r => r.Id)
                .ToListAsync();


            var viewModel = new ProductDetailsViewModel
            {
                ProductDetails = productById,
                Ratings = ratings
            };


            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> CommentProduct([FromForm] RatingModel rating)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage);
                return BadRequest(new { message = string.Join(", ", errors) });
            }

            var ratingEntity = new RatingModel
            {
                ProductId = rating.ProductId,
                Name = rating.Name,
                Email = rating.Email,
                Comment = rating.Comment,
                Star = rating.Star
            };

            _dataContext.Rating.Add(ratingEntity);
            await _dataContext.SaveChangesAsync();

            return Json(new { message = "Đánh giá đã được gửi thành công!" });
        }
    }
}
