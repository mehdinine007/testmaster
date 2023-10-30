using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportManagement.Application.Contracts.ReportManagement.Dtos
{
    public class DashboardWidgetCreateOrUpdateDto
    {
        public int Id { get; set; }
        public int DashboardId { get; set; }
        public int WidgetId { get; set; }
       
    }
}
