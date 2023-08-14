using OrderManagement.Domain.Shared;

namespace OrderManagement.Application.Contracts
{
    public class ProductAndCategoryGetListQueryDto
    {
        public ProductAndCategoryType Type { get; set; }
        public string NodePath { get; set; }
        public int? ProductLevelId { get; set; }
        public List<PropertyFilter> PropertyFilters { get; set; }
    }

}
