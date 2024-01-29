using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanelManagement.Application.Contracts.AdminPanelManagement.Dtos.report
{
    public class ChartInputDto
    {
        public int WidgetId { get; set; }
        public List<ConditionValue> ConditionValue { get; set; }
    }
}
