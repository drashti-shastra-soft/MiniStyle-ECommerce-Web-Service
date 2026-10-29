using System;

namespace MiniStyleWebService.Domain
{
    public class Product : AuditTrail
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ShortInfo { get; set; }
        public int SubCategoryId { get; set; }
        public int? MicroCategoryId { get; set; }
        public bool IsActive { get; set; }
        public string SubCategory { get; set; }
        public string Category { get; set; }
        public string MicroCategory { get; set; }
        public string Status { get; set; }
        public int CategoryId { get; set; }
        public string ImageBase64String { get; set; }
        public string ImageFileName { get; set; }
        public decimal SellPrice { get; set; }
        public decimal DiscountedPrice { get; set; }
        public int DiscountPercentage { get; set; }
    }
}
