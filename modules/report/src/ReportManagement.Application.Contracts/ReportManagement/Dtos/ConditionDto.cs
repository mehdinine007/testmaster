using IFG.Core.Attributes;
using ReportManagement.Domain.Shared.ReportManagement.Enums;


namespace ReportManagement.Application.Contracts.ReportManagement.Dtos
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
        Post=2
    }
}
