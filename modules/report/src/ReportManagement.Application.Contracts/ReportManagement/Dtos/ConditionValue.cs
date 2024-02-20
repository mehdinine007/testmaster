using ReportManagement.Domain.Shared.ReportManagement.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportManagement.Application.Contracts.ReportManagement.Dtos
{
    public class ConditionValue
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public List<string> Values { get; set; }
        public ConditionTypeEnum Type { get; set; }
        public bool MultiSelect { get; set; } = false;
    }
}
