using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniStyleWebService.Domain
{
    public class SizeChart
    {
        public int RowId { get; set; }
        public int ProductAgeGroupId { get; set; }
        public string AgeGroupName { get; set; }
        public Dictionary<string, decimal> Measurement { get; set; } = new Dictionary<string, decimal>();
    }
}
