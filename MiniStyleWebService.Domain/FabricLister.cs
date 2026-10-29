using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniStyleWebService.Domain
{
    public class FabricLister
    {
        public List<Fabric> List { get; set; } = new List<Fabric>();

        public Fabric SearchCriteria { get; set; } = new Fabric();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
