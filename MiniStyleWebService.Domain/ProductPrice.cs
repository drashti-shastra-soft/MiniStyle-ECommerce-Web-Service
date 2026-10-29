using System;


namespace MiniStyleWebService.Domain
{
    public class ProductPrice
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SellPrice { get; set; }
        public decimal DiscountedPrice { get; set; }
        public int DiscountPercentage { get; set; }
    }
}
