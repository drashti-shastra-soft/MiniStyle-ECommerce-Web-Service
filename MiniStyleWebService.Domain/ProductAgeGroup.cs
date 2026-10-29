using System;
using System.Linq;

namespace MiniStyleWebService.Domain
{
    public class ProductAgeGroup
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int AgeGroupId { get; set; }
        public string AgeGroup { get; set; }
    }
}
