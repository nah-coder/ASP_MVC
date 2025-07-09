﻿namespace Shoppng_Tutorial.Models
{
    public class OrderModel
    {
        public int Id { get; set; }
        public string OrderCode { get; set; }
        public decimal ShippingCost { get; set; }
        public string? PaymentMethod { get; set; }
        public string UserName { get; set; }
        public string CouponCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }
        public string ShippingAddress { get; set; }
        public decimal GrandTotal { get; set; }

    }
}
