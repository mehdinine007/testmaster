using OrderManagement.Domain.Shared;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class ProductAndCategoryGetListQueryDto
    {
        public ProductAndCategoryType Type { get; set; }

        public string NodePath { get; set; }
    }
}
