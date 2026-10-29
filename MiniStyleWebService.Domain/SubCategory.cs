using System;

namespace MiniStyleWebService.Domain
{
    public class SubCategory : AuditTrail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public int AuditId { get; set; }
    }
}
