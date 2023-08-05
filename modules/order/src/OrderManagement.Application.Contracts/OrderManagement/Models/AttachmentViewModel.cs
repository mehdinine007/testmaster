using OrderManagement.Domain.Shared;

namespace OrderManagement.Application.Contracts
{
    public class AttachmentViewModel
    {
        public string Title { get; set; }
        public string FileName { get; set; }
        public AttachmentEntityTypeEnum Type { get; set; }
        public string TypeTitle { get; set; }
        public AttachmentLocationEnum Location { get; set; }
        public string LocationTitle { get; set; }
        public string Description { get; set; }
        public List<string> Content { get; set; }
        public int Priority { get; set; }
        public int EntityId { get; set; }

    }
}
