using System.Collections.Generic;

namespace MiniStyleWebService.Domain
{
    public class ColorLister
    {
        public List<Color> List { get; set; } = new List<Color>();

        public Color SearchCriteria { get; set; } = new Color();

        public Pagination Pagination { get; set; } = new Pagination();

    }
}
