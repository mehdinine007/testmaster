using AdminPanelManagement.Domain.Shared.AdminPanelManagement.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanelManagement.Application.Contracts.AdminPanelManagement.Dtos.report
{
    public class ChartDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public WidgetTypeEnum Type { get; set; }
        public List<CategoryData> Categories { get; set; }
        public List<ChartSeriesData> Series { get; set; }
    }
}
