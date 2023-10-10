using ReportManagement.Domain.Shared.ReportManagement.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportManagement.Application.Contracts.ReportManagement.Dtos
{
    public class WidgetCreateOrUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public WidgetTypeEnum Type { get; set; }
        public string Command { get; set; }
        public List<FieldsDto> Fields { get; set; }
        public List<ConditionDto> Condition { get; set; }
    }
}
