using OrderManagement.Domain.Shared;

namespace OrderManagement.Application.Contracts
{
    public class AttachmentViewModel
    {
        public string Title { get; set; }
        public string FileName { get; set; }
        public AttachmentEntityTypeEnum Type { get; set; }
        public string TypeTitle { get; set; }
        public string Description { get; set; }

    }
}
