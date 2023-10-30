using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportManagement.Domain.Shared.ReportManagement.Dtos
{
    public class ChartSeriesData
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public List<long> Data { get; set; }
    }
}
