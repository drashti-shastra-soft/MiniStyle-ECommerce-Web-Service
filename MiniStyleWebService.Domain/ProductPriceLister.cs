using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniStyleWebService.Domain
{
    public class ProductPriceLister
    {
        public List<ProductPrice> List { get; set; } = new List<ProductPrice>();

        public ProductPrice SearchCriteria { get; set; } = new ProductPrice();
    }
}
