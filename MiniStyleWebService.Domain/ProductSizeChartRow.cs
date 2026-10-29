using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniStyleWebService.Domain
{
    public class ProductSizeChartRow
    {
        public int Id { get; set; }
        public int ProductSizeChartId { get; set; }
        public int ProductAgeGroupId { get; set; }
        public int OrderIndex { get; set; }

        public List<ProductSizeChartMeasurement> SizeChartMeasurements = new List<ProductSizeChartMeasurement>();
    }
}
