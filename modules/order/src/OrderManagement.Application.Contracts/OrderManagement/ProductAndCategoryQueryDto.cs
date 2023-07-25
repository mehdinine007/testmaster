using Volo.Abp.Application.Dtos;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class ProductAndCategoryQueryDto : PagedResultRequestDto
    {
        public int ParentId { get; set; }

        public int AttachmentEntityType { get; set; }
    }
}
