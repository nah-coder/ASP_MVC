using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shoppng_Tutorial.Models;
using Shoppng_Tutorial.Repository;

namespace Shoppng_Tutorial.Controllers
{
    public class HomeController : Controller
    {

        private readonly DataContext _dataContext;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUserModel> _userManager;

        public HomeController(ILogger<HomeController> logger, DataContext context, UserManager<AppUserModel> userManager)
        {
            _logger = logger;
            _dataContext = context;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            var products = _dataContext.Products.Include("Category").Include("Brand").ToList();
            var sliders = _dataContext.Sliders.Where(s => s.Status == 1).ToList();
            ViewBag.Sliders = sliders;
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Contact()
        {
            var contact = await _dataContext.Contact.FirstOrDefaultAsync();
            return View(contact);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(int statuscode)
        {
            if (statuscode == 404)
            {
                return View("NotFound");
            }
            else
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

        }

        public async Task<IActionResult> Compare()
        {
            var compare_product = await (from c in _dataContext.Compare
                                         join p in _dataContext.Products on c.ProductId equals p.Id
                                         join u in _dataContext.Users on c.UserId equals u.Id
                                         select new { User = u, Product = p, Compares = c })
                               .ToListAsync();

            return View(compare_product);
        }
        public async Task<IActionResult> DeleteCompare(int Id)
        {
            CompareModel compare = await _dataContext.Compare.FindAsync(Id);

            _dataContext.Compare.Remove(compare);

            await _dataContext.SaveChangesAsync();
            //TempData["success"] = "So sánh ?ã ???c xóa thành công";
            return RedirectToAction("Compare", "Home");
        }
        public async Task<IActionResult> DeleteWishlist(int Id)
        {
            WishlistModel wishlist = await _dataContext.Wishlist.FindAsync(Id);

            _dataContext.Wishlist.Remove(wishlist);

            await _dataContext.SaveChangesAsync();
            //TempData["success"] = "Yêu thích ?ã ???c xóa thành công";
            return RedirectToAction("Wishlist", "Home");
        }
        public async Task<IActionResult> wishlist()
        {
            var wishlist_product = await (from w in _dataContext.Wishlist
                                          join p in _dataContext.Products on w.ProductId equals p.Id
                                          select new { product = p, wishlists = w })
                               .ToListAsync();

            return View(wishlist_product);
        }

        [HttpPost]
        public async Task<IActionResult> AddWishlist(long Id, WishlistModel wishlistmodel)
        {
            var user = await _userManager.GetUserAsync(User);

            var wishlistProduct = new WishlistModel
            {
                ProductId = Id,
                UserId = user.Id
            };

            _dataContext.Wishlist.Add(wishlistProduct);
            try
            {
                await _dataContext.SaveChangesAsync();
                return Ok(new { success = true, message = "Thêm vào danh sách yêu thích thành công" });
            }
            catch (Exception)
            {
                return StatusCode(500, "?ã x?y ra l?i khi thêm vào danh sách yêu thích.");
            }

        }
        [HttpPost]
        public async Task<IActionResult> AddCompare(long Id)
        {
            var user = await _userManager.GetUserAsync(User);

            var compareProduct = new CompareModel
            {
                ProductId = Id,
                UserId = user.Id
            };

            _dataContext.Compare.Add(compareProduct);
            try
            {
                await _dataContext.SaveChangesAsync();
                return Ok(new { success = true, message = "Thêm vào danh sách so sánh thành công" });
            }
            catch (Exception)
            {
                return StatusCode(500, "?ã x?y ra l?i khi thêm vào danh sách so sánh.");
            }
        }
    }
}
