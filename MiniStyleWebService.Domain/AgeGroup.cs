using System;

namespace MiniStyleWebService.Domain
{
    public class AgeGroup : AuditTrail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int AuditId { get; set; }
        public string Status { get; set; }
    }
}
