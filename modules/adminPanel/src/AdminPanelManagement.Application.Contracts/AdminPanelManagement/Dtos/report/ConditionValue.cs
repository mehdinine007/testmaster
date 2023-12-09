using AdminPanelManagement.Domain.Shared.AdminPanelManagement.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanelManagement.Application.Contracts.AdminPanelManagement.Dtos.report
{
    public class ConditionValue
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public ConditionTypeEnum Type { get; set; }
        public bool MultiSelect { get; set; } = false;
    }
}
