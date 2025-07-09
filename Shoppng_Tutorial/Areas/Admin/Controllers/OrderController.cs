using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shoppng_Tutorial.Repository;

namespace Shoppng_Tutorial.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly DataContext _dataContext;
        public OrderController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _dataContext.Orders.OrderByDescending(p => p.Id).ToListAsync());
        }


        //[HttpGet]
        //[Route("PaymentMomoInfo")]
        //public async Task<IActionResult> PaymentMomoInfo(string orderId)
        //{
        //    var momoInfo = await _dataContext.MomoInfos.FirstOrDefaultAsync(m => m.OrderId == orderId);

        //    if (momoInfo == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(momoInfo);
        //}

        //[HttpGet]
        //[Route("PaymentVnpayInfo")]
        //public async Task<IActionResult> PaymentVnpayInfo(string orderId)
        //{
        //    var vnpayInfo = await _dataContext.VnInfos.FirstOrDefaultAsync(v => v.PaymentId == orderId);

        //    if (vnpayInfo == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(vnpayInfo);
        //}
        public async Task<IActionResult> ViewOrder(string ordercode)
        {
            var DetailsOrder = await _dataContext.OrderDetail.Include(od => od.Product).Where(od => od.OrderCode == ordercode).ToListAsync();

            //lấy shipping cost
            //var Order = _dataContext.Orders.Where(o => o.OrderCode == ordercode).First();
            //ViewBag.ShippingCost = Order.ShippingCost;
            //ViewBag.Status = Order.Status;

            //ViewBag.Coupon = Order.CouponCode;
            //decimal discountAmount = 0;
            //if (!string.IsNullOrEmpty(Order.CouponCode))
            //{
            //    var coupon = await _dataContext.Coupons
            //        .FirstOrDefaultAsync(c => (c.Name + " | " + c.Description) == Order.CouponCode);

            //    discountAmount = coupon.Price;
            //}

            //ViewBag.DiscountAmount = discountAmount;

            return View(DetailsOrder);
        }

        //[HttpPost]
        //[Route("UpdateOrder")]
        //public async Task<IActionResult> UpdateOrder(string ordercode, int status)
        //{
        //    var order = await _dataContext.Orders.FirstOrDefaultAsync(o => o.OrderCode == ordercode);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    int oldStatus = order.Status; // Lưu trạng thái cũ
        //    order.Status = status;
        //    _dataContext.Update(order);

        //    var DetailsOrder = await _dataContext.OrderDetails
        //        .Include(od => od.Product)
        //        .Where(od => od.OrderCode == order.OrderCode)
        //        .Select(od => new
        //        {
        //            od.Quantity,
        //            od.Product.Price,
        //            od.Product.CapitalPrice
        //        })
        //        .ToListAsync();

        //    var statisticalModel = await _dataContext.Statisticals
        //        .FirstOrDefaultAsync(s => s.DateCreated.Date == order.CreatedDate.Date);

        //    // Nếu trước đó là "Đã giao hàng" (2) nhưng giờ chuyển về "Đơn hàng mới" (1) hoặc "Đã hủy" (3) thì xóa dữ liệu đã lưu
        //    if (oldStatus == 2 && (status == 1 || status == 0))
        //    {
        //        if (statisticalModel != null)
        //        {
        //            foreach (var orderDetail in DetailsOrder)
        //            {
        //                statisticalModel.Quantity -= 1;
        //                statisticalModel.Sold -= orderDetail.Quantity;
        //                statisticalModel.Revenue -= orderDetail.Quantity * orderDetail.Price;
        //                statisticalModel.Profit -= (orderDetail.Price - orderDetail.CapitalPrice) * orderDetail.Quantity;
        //            }

        //            // Nếu không còn đơn nào trong ngày thì xóa dữ liệu
        //            if (statisticalModel.Quantity <= 0)
        //            {
        //                _dataContext.Statisticals.Remove(statisticalModel);
        //            }
        //            else
        //            {
        //                _dataContext.Update(statisticalModel);
        //            }
        //        }
        //    }

        //    // Nếu trạng thái mới là "Đã giao hàng" (2) thì thêm dữ liệu vào thống kê
        //    if (status == 2)
        //    {
        //        if (statisticalModel != null)
        //        {
        //            foreach (var orderDetail in DetailsOrder)
        //            {
        //                statisticalModel.Quantity += 1;
        //                statisticalModel.Sold += orderDetail.Quantity;
        //                statisticalModel.Revenue += orderDetail.Quantity * orderDetail.Price;
        //                statisticalModel.Profit += (orderDetail.Price - orderDetail.CapitalPrice) * orderDetail.Quantity;
        //            }
        //            _dataContext.Update(statisticalModel);
        //        }
        //        else
        //        {
        //            int new_quantity = 0;
        //            int new_sold = 0;
        //            decimal new_profit = 0;
        //            decimal new_revenue = 0;

        //            foreach (var orderDetail in DetailsOrder)
        //            {
        //                new_quantity += 1;
        //                new_sold += orderDetail.Quantity;
        //                new_profit += (orderDetail.Price - orderDetail.CapitalPrice) * orderDetail.Quantity;
        //                new_revenue += orderDetail.Quantity * orderDetail.Price;
        //            }

        //            statisticalModel = new StatisticalModel
        //            {
        //                DateCreated = order.CreatedDate,
        //                Quantity = new_quantity,
        //                Sold = new_sold,
        //                Revenue = new_revenue,
        //                Profit = new_profit
        //            };

        //            _dataContext.Add(statisticalModel);
        //        }
        //    }

        //    try
        //    {
        //        await _dataContext.SaveChangesAsync();
        //        return Ok(new { success = true, message = "Trạng thái đơn hàng đã cập nhật thành công" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "Lỗi khi cập nhật trạng thái đơn hàng");
        //    }
        //}


        //[HttpGet]
        //[Route("Delete")]
        //public async Task<IActionResult> Delete(string ordercode)
        //{
        //    var order = await _dataContext.Orders.FirstOrDefaultAsync(o => o.OrderCode == ordercode);

        //    if (order == null)
        //    {
        //        return NotFound(new { success = false, message = "Không tìm thấy đơn hàng" });
        //    }
        //    try
        //    {
        //        // Xóa các OrderDetails liên quan trước khi xóa Order
        //        var orderDetails = _dataContext.OrderDetails.Where(od => od.OrderCode == ordercode);
        //        _dataContext.OrderDetails.RemoveRange(orderDetails);

        //        // Xóa Order
        //        _dataContext.Orders.Remove(order);

        //        await _dataContext.SaveChangesAsync();

        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { success = false, message = "Lỗi khi xóa đơn hàng", error = ex.Message });
        //    }
        //}

    }
}
