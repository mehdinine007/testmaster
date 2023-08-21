using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class ChartStructureDto
    {
        public string Title { get; set; }
        public ChartTypeEnum Type { get; set; }
        public string TypeTitle { get; set; }
        public List<string> Categories { get; set; }
        public List<ChartSeriesData> Series { get; set; }
        public int Priority { get; set; }

    }

    public class ChartSeriesData
    {
        public string Name { get; set; }
        public List<int> Data { get; set; }
    }

}
