using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class ChartSeries 
    {
        public List<string> SeriesTitle { get; set; }
        public List<ChartSeriesData> SeriesData { get; set; }
    }

    public class ChartSeriesData
    {
        public string Name { get; set; }
        public List<int> Data { get; set; }
    }

}
