namespace Shoppng_Tutorial.Models.ViewModel
{
    public class CartItemViewModel
    {
        public List<CartItemModel> CartItems { get; set; }
        public decimal GrandTotal { get; set; }
        public decimal ShippingPrice { get; set; }
        public decimal DiscountAmount { get; set; }
        public string CouponCode { get; set; }
        public decimal TotalProductPrice => CartItems.Sum(x => x.Quantity * x.Price);


    }
}
