using System.Collections.Generic;

namespace MiniStyleWebService.Domain
{
    public class ProductLister
    {
        public List<Product> List { get; set; } = new List<Product>();

        public Product SearchCriteria { get; set; } = new Product();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
