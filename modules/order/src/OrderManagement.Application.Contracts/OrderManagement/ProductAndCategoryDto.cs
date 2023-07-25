using OrderManagement.Domain.Shared;

namespace OrderManagement.Application.Contracts
{
    public class ProductAndCategoryDto
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Title { get; set; }

        public int? ParentId { get; set; }

        public ProductAndCategoryType Type { get; set; }

        public int LevelId { get; set; }

        public bool HasChild { get; set; }

        public List<AttachmentViewModel> Attachments { get; set; }
    }
}