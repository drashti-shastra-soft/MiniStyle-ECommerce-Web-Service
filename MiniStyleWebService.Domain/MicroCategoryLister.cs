using System.Collections.Generic;

namespace MiniStyleWebService.Domain
{
    public class MicroCategoryLister
    {
        public List<MicroCategory> List { get; set; } = new List<MicroCategory>();

        public MicroCategory SearchCriteria { get; set; } = new MicroCategory();

        public Pagination Pagination { get; set; } = new Pagination();
    }
}
