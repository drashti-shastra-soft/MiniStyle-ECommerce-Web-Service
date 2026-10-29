using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniStyleWebService.Domain
{
    public class OrderLine
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string Product { get; set; }
        public string ProductCode { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public int Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? TotalPrice { get; set; }
    }
}
