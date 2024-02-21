using OrderManagement.Domain.Shared;

namespace OrderManagement.Application.Contracts
{
    public class ProductAndCategoryGetListQueryDto
    {
        public ProductAndCategoryType Type { get; set; }
        public string NodePath { get; set; }
        public int? ProductLevelId { get; set; }
        public List<AdvancedSearchDto> AdvancedSearch { get; set; }
        public  string attachmentType { get; set; }
        public string attachmentlocation { get; set; }
        public bool HasProperty { get; set; }
        public bool IsActive { get; set; }= true;
        public int? OrganizationId { get; set; }

    }

}
