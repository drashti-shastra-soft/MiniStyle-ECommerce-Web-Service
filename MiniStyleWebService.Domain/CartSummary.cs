using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniStyleWebService.Domain
{
    public class CartSummary
    {
        public decimal TotalPrice { get; set; }
        public decimal Discounted { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal Total { get; set; }
        public string TotalPriceFormatted => string.Format(new CultureInfo("en-IN"), "{0:C2}", TotalPrice);
        public string DiscountedFormatted => string.Format(new CultureInfo("en-IN"), "{0:C2}", Discounted);
        public string ShippingFeeFormatted => string.Format(new CultureInfo("en-IN"), "{0:C2}", ShippingFee);
        public string TotalFormatted => string.Format(new CultureInfo("en-IN"), "{0:C2}", Total);
    }
}
