namespace OrderManagement.Application.Contracts
{
    public class AdvancedSearchDto
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public OperatorFilterEnum Operator { get; set; }
    }
}
