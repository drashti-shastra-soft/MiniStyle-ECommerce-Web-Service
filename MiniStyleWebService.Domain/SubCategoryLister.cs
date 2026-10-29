using System.Collections.Generic;

namespace MiniStyleWebService.Domain
{
    public class SubCategoryLister
    {
        public List<SubCategory> List { get; set; } = new List<SubCategory>();

        public SubCategory SearchCriteria { get; set; } = new SubCategory();

        public Pagination Pagination { get; set; } = new Pagination();


    }
}
