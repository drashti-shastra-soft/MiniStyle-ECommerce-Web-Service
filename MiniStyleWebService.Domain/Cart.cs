using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniStyleWebService.Domain
{
    public class Cart
    {
        public int Id { get; set; }
        public string SessionId { get; set; }
        public string ProductName { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string FileName { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string ImageBase64String { get; set; }
        public int Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal DiscountedPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public string UnitPriceFormatted => string.Format(new CultureInfo("en-IN"), "{0:C2}", UnitPrice);
        public string DiscountedPriceFormatted => string.Format(new CultureInfo("en-IN"), "{0:C2}", DiscountedPrice);
    }
}
