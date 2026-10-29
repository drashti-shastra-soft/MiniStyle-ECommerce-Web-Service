using System;
using System.Linq;

namespace MiniStyleWebService.Domain
{
    public class Fabric : AuditTrail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int AuditId { get; set; }
        public string Status { get; set; }
    }
}
