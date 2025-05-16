using Microsoft.AspNetCore.Mvc;

namespace Shoppng_Tutorial.Controllers
{
    public class CategoryController:Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
