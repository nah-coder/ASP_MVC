using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shoppng_Tutorial.Models;
using Shoppng_Tutorial.Models.ViewModel;
using Shoppng_Tutorial.Repository;

namespace Shoppng_Tutorial.Controllers
{
    public class CartController : Controller
    {
        private readonly DataContext _dataContext;

        public CartController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public IActionResult Index()
        {

            List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();

            var shippingPriceCookie = Request.Cookies["ShippingPrice"];
            decimal shippingPrice = 0;

            if (shippingPriceCookie != null)
            {
                shippingPrice = JsonConvert.DeserializeObject<decimal>(shippingPriceCookie);
            }

            var couponCode = Request.Cookies["CouponTitle"];
            decimal couponDiscount = 0;

            //if (!string.IsNullOrEmpty(couponCode))
            //{
            //    var coupon = _dataContext.Coupons.FirstOrDefault(x => (x.Name + " | " + x.Description) == couponCode);
            //    if (coupon != null && coupon.DateExpired >= DateTime.Now && coupon.Quantity > 0)
            //    {
            //        couponDiscount = coupon.Price;
            //    }
            //}

            decimal total = cartItems.Sum(x => x.Quantity * x.Price) + shippingPrice;
            total -= couponDiscount;

            // Không cho tổng tiền nhỏ hơn 0
            if (total < 0)
                total = 0;

            HttpContext.Session.SetString("GrandTotal", total.ToString());


            CartItemViewModel cartVM = new()
            {
                CartItems = cartItems,
                GrandTotal = total,
                ShippingPrice = shippingPrice,
                CouponCode = couponCode,
                DiscountAmount = couponDiscount
            };

            return View(cartVM);
        }
        public IActionResult Checkout()
        {
            return View("~/Views/Checkout/Index.cshtml");
        }

        public async Task<IActionResult> AddCart(long Id)
        {
            ProductModel product = await _dataContext.Products.FindAsync(Id);
            List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            CartItemModel cartItem = cartItems.Where(c => c.ProductId == Id).FirstOrDefault();

            if(cartItem == null)
            {
                cartItems.Add(new CartItemModel(product));
            }
            else
            {
                cartItem.Quantity++;
            }
            HttpContext.Session.SetJson("Cart", cartItems);
            TempData["success"] = "Thêm sản phẩm vào giỏ hàng thành công";
            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> Decrease(int Id)
        {
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");

            CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();

            if (cartItem.Quantity > 1)
            {
                --cartItem.Quantity;
            }
            else
            {
                cart.RemoveAll(p => p.ProductId == Id);
            }
            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            TempData["success"] = "Giảm số lượng sản phẩm trong giỏ hàng thành công";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Increase(int Id)
        {
            ProductModel product = await _dataContext.Products.Where(p => p.Id == Id).FirstOrDefaultAsync();

            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");

            CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();

            if (cartItem.Quantity >= 1 && product.Quantity > cartItem.Quantity)
            {
                ++cartItem.Quantity;
                //TempData["success"] = "Tăng số lượng sản phẩm trong giỏ hàng thành công ";
            }
            else
            {
                cartItem.Quantity = product.Quantity;
                TempData["success"] = "Đã đạt số lượng tối đa cho sản phẩm trong giỏ hàng!";
                //cart.RemoveAll(p => p.ProductId == Id);
            }
            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            TempData["success"] = "Tăng số lượng sản phẩm trong giỏ hàng thành công";

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Remove(int Id)
        {
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");

            cart.RemoveAll(p => p.ProductId == Id);

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            TempData["success"] = "Xóa sản phẩm thành công";

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Clear()
        {
            HttpContext.Session.Remove("Cart");
            TempData["success"] = "Xóa toàn bộ sả phẩm thành công";
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Add(long Id)
        {
            ProductModel product = await _dataContext.Products.FindAsync(Id);
            List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
            CartItemModel cartItems = cart.Where(c => c.ProductId == Id).FirstOrDefault();

            if (cartItems == null)
            {
                cart.Add(new CartItemModel(product));
            }
            else
            {
                cartItems.Quantity += 1;
            }

            HttpContext.Session.SetJson("Cart", cart); //luu tru dl cart vao session

            TempData["success"] = "Thêm sản phẩm vào giỏ hàng thành công";

            return Redirect(Request.Headers["Referer"].ToString());
        }
        [HttpPost]
        public async Task<IActionResult> GetShipping(ShippingModel shippingModel, string quan, string tinh, string phuong)
        {
            string fullAddress = $"{phuong}, {quan}, {tinh}";

            var existingShipping = await _dataContext.Shippings
                .FirstOrDefaultAsync(x => x.City == tinh && x.District == quan && x.Ward == phuong);

            decimal shippingPrice = 0; // Set mặc định giá tiền

            if (existingShipping != null)
            {
                shippingPrice = existingShipping.Price;
            }
            else
            {
                //Set mặc định giá tiền nếu ko tìm thấy
                shippingPrice = 30000;
            }
            var shippingPriceJson = JsonConvert.SerializeObject(shippingPrice);
            try
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(30),
                    Secure = true // using HTTPS
                };


                HttpContext.Session.SetString("ShippingAddress", fullAddress);

                Response.Cookies.Append("ShippingPrice", shippingPriceJson, cookieOptions);
            }
            catch (Exception ex)
            {
                //
                Console.WriteLine($"Lỗi khi thêm cookie chứa giá vận chuyển: {ex.Message}");
            }
            return Json(new { shippingPrice });
        }


        [HttpGet]
        public IActionResult DeleteShipping()
        {
            Response.Cookies.Delete("ShippingPrice");
            return Json(new { success = true, message = "Xóa phí vận chuyển thành công" });
        }

    }
}
