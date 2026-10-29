using System;

namespace MiniStyleWebService.Domain
{
    public class Color : AuditTrail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HashCode { get; set; }
        public bool IsActive { get; set; }
        public int AuditId { get; set; }
        public string Status { get; set; }
    }
}
