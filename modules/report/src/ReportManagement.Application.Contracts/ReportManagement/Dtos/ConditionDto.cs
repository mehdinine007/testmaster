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
    }
}
