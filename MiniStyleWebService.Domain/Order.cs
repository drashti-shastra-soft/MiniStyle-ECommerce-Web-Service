using System;
using System.Linq;

namespace MiniStyleWebService.Domain
{
    public class Order
    {
        public int Id { get; set; }
        public string SessionId { get; set; }
        public string FullName { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Landmark { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string ShippingStatus { get; set; }
        public string RazorpayPaymentId { get; set; }
        public string RazorpayOrderId { get; set; }
    }
}
