using Microsoft.AspNetCore.Mvc;

namespace Shoppng_Tutorial.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details()
        {
            return View();
        }
    }
}
