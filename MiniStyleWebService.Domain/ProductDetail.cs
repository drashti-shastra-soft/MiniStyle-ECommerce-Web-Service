namespace MiniStyleWebService.Domain
{
    public class ProductDetail
    {
        public int ProductDetailId { get; set; }
        public int ProductId { get; set; }
        public string Description { get; set; }
        public string Specification { get; set; }
        public int FabricId { get; set; }
        public string Fabric { get; set; }
    }
}
