using System;

namespace MiniStyleWebService.Domain
{
    public class ProductImage
    {
        public int Id { get; set; }
        public int ProductColorId { get; set; }
        public int ProductId { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public string ImagePath { get; set; }
        public string Base64String { get; set; }
    }
}
