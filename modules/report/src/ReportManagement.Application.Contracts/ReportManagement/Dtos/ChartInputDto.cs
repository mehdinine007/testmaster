using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportManagement.Application.Contracts.ReportManagement.Dtos
{
    public class ChartInputDto
    {
        public int WidgetId { get; set; }
        public List<ConditionValue> ConditionValue { get; set; }
    }
}
