using ReportManagement.Domain.Shared.ReportManagement.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportManagement.Application.Contracts.ReportManagement.Dtos
{
    public class WidgetDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public WidgetTypeEnum Type { get; set; }

        public List<ConditionDto> Condition { get; set; }
  
    }
}
