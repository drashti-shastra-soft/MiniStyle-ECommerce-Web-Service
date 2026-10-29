using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniStyleWebService.Domain
{
    public class ProductViewModel
    {
        public Domain.Product Product { get; set; } = new Product();

        public Domain.ProductDetail ProductDetail { get; set; } = new ProductDetail();

        public Domain.ProductPrice ProductPrice { get; set; } = new ProductPrice();

        public List<ProductAgeGroup> AgeGroup { get; set; } = new List<ProductAgeGroup>();

        public List<ProductColor> Color { get; set; } = new List<ProductColor>();
        
        public List<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
    }
}
