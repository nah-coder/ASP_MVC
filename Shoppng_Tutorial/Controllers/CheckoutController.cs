using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Shoppng_Tutorial.Areas.Admin.Repository;
using Shoppng_Tutorial.Models;
using Shoppng_Tutorial.Repository;

namespace Shoppng_Tutorial.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IEmailSender _emailSender;
        public CheckoutController(IEmailSender emailSender, DataContext context)
        {
            _dataContext = context;
            _emailSender = emailSender;
            //_momoService = momoService;
            //_vnPayService = vnPayService;
        }


        public async Task<IActionResult> Checkout(string PaymentMethod, string PaymentId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (userEmail == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                //tạo đơn hàng mới
                var ordercode = Guid.NewGuid().ToString();
                var orderItem = new OrderModel();
                orderItem.OrderCode = ordercode;

                // Nhận shipping giá từ cookie
                var shippingPriceCookie = Request.Cookies["ShippingPrice"];
                decimal shippingPrice = 0;

                //Nhận Coupon code từ cookie
                //var coupon_code = Request.Cookies["CouponTitle"];

                //Nhận Coupon code từ Session
                var coupon_code = HttpContext.Session.GetString("CouponCode");

                //Nhận ShippingAddress từ Session
                var shippingAddress = HttpContext.Session.GetString("ShippingAddress") ?? "";

                if (shippingPriceCookie != null)
                {
                    var shippingPriceJson = shippingPriceCookie;
                    shippingPrice = JsonConvert.DeserializeObject<decimal>(shippingPriceJson);
                }
                else
                {
                    shippingPrice = 0;
                }

                var grandTotalString = HttpContext.Session.GetString("GrandTotal");
                decimal grandTotal = 0;
                if (!string.IsNullOrEmpty(grandTotalString))
                {
                    grandTotal = decimal.Parse(grandTotalString);
                }
                orderItem.GrandTotal = grandTotal; // Cần thêm field này trong model


                orderItem.ShippingAddress = shippingAddress;
                orderItem.ShippingCost = shippingPrice;
                orderItem.CouponCode = coupon_code;
                orderItem.UserName = userEmail;
                //orderItem.PaymentMethod = PaymentMethod + " " + PaymentId;
                // Nếu không truyền PaymentMethod, mặc định là COD
                if (string.IsNullOrEmpty(PaymentMethod))
                {
                    PaymentMethod = "COD";
                    PaymentId = ""; // không có mã giao dịch
                }
                orderItem.PaymentMethod = PaymentMethod + " " + PaymentId;


                orderItem.Status = 1;
                orderItem.CreatedDate = DateTime.Now;
                _dataContext.Add(orderItem);
                _dataContext.SaveChanges();

                //chi tiết đơn hàng
                List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
                foreach (var cart in cartItems)
                {
                    var orderdetails = new OrderDetail();
                    orderdetails.UserName = userEmail;
                    orderdetails.OrderCode = ordercode;
                    orderdetails.ProductId = cart.ProductId;
                    orderdetails.Price = cart.Price;
                    orderdetails.Quantity = cart.Quantity;
                    //update product quantity
                    var product = await _dataContext.Products.Where(p => p.Id == cart.ProductId).FirstAsync();
                    product.Quantity -= cart.Quantity;
                    product.Sold += cart.Quantity;
                    _dataContext.Update(product);
                    //++update product quantity
                    _dataContext.Add(orderdetails);
                    _dataContext.SaveChanges();
                }
                HttpContext.Session.Remove("Cart");
                //Send mail order when success
                //var receiver = "nhhuy.dhti15a3hn@sv.uneti.edu.vn";
                var receiver = userEmail;
                var subject = "Đặt hàng thành công";
                var message = "Đặt hàng thành công, trải nghiệm dịch vụ nhé.";

                await _emailSender.SendEmailAsync(receiver, subject, message);

                TempData["success"] = "Đặt hàng thành công, vui lòng chờ duyệt đơn hàng";
                return RedirectToAction("Index", "Cart");

            }
            return View();
        }

    }
}
