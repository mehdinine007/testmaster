using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanelManagement.Application.Contracts.AdminPanelManagement.Dtos.report
{
    public class ChartSeriesData
    {

        public string Name { get; set; }
        public string Color { get; set; }
        public List<long> Data { get; set; }
    }
}
