using OrderManagement.Domain.Shared;
using Volo.Abp.Application.Dtos;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class ProductAndCategoryQueryDto : PagedResultRequestDto
    {
        public int? ParentId { get; set; }

        public string? AttachmentType { get; set; } = null;
        public string? AttachmentLocation { get; set; } = null;
    }
}
