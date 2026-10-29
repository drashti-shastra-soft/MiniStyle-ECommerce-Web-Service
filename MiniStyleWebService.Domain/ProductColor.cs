using System;
using System.Linq;

namespace MiniStyleWebService.Domain
{
    public class ProductColor
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ColorId { get; set; }
        public string Color { get; set; }
        public string HashCode { get; set; }
    }
}
