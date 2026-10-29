using System;

namespace MiniStyleWebService.Domain
{
    public class Category : AuditTrail 
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
