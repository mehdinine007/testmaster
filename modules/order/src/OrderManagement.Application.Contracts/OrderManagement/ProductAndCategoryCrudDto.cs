using OrderManagement.Domain.Shared;

namespace OrderManagement.Application.Contracts
{
    public class ProductAndCategoryCreateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? ParentId { get; set; }
        public ProductAndCategoryType Type { get; set; }
        public bool Active { get; set; }
        public int Priority { get; set; }
        public int OrganizationId { get; set; }
    }
}