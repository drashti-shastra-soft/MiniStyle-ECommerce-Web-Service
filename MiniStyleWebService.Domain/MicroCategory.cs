using System;

namespace MiniStyleWebService.Domain
{
    public class MicroCategory : AuditTrail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SubCategoryId { get; set; }
        public bool IsActive { get; set; }
        public int AuditId { get; set; }
        public string Status { get; set; }
        public string SubCategory { get; set; }
    }
}
