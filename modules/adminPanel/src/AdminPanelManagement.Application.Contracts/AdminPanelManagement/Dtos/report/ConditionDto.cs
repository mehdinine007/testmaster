using AdminPanelManagement.Domain.Shared.AdminPanelManagement.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanelManagement.Application.Contracts.AdminPanelManagement.Dtos.report
{
    public class ConditionDto
    {
        public string Title { get; set; }
        public ConditionTypeEnum Type { get; set; }
        public string Name { get; set; }
        public string Default { get; set; }
        public int Priority { get; set; }
        public List<DrowpDownItem> DrowpDownItems { get; set; }
        public bool MultiSelect { get; set; } = false;
        public ApiContent ApiContent { get; set; }

    }

    public class DrowpDownItem
    {
        public int Value { get; set; }
        public string Title { get; set; }
    }

    public class ApiContent
    {
        public ApiCallType Type { get; set; }
        public string Url { get; set; }
        public string Body { get; set; }
    }

    public enum ApiCallType
    {
        Get = 1,
        Post = 2
    }
}
