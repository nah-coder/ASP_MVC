using Microsoft.AspNetCore.Mvc;

namespace Shoppng_Tutorial.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Checkout()
        {
            return View("~/Views/Checkout/Index.cshtml");
        }
    }
   
}
