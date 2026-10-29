using System;
using System.Linq;

namespace MiniStyleWebService.Domain
{
    public class ProductSizeChart : AuditTrail
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ProductAgeGroupId { get; set; }
        public string WearablePart { get; set; }
        public string AgeGroup { get; set; }
        public string UnitType { get; set; }
        public string Number { get; set; }
        public string Shoulder { get; set; }
        public string Length { get; set; }
        public string SleeveLength { get; set; }
        public string Chest { get; set; }
        public string Waist { get; set; }

        public int AuditId { get; set; }
    }
}
