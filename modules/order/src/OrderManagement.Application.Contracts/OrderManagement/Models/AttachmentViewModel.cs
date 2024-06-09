using OrderManagement.Domain.Shared;
using OrderManagement.Domain.Shared.OrderManagement.Enums;

namespace OrderManagement.Application.Contracts
{
    public class AttachmentViewModel
    {
        public Guid Id { get; set; }
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
        public DeviceEnum Device { get; set; }
        public string DeviceTitle { get; set; }
        public int VersionNumber { get; set; }

    }
}
