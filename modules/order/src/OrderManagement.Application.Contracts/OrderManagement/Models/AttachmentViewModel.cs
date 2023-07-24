using Esale.Core.Utility.Tools;
using OrderManagement.Domain.Shared;

namespace OrderManagement.Application.Contracts
{
    public class AttachmentViewModel
    {
        public string Title { get; set; }
        public string FileName { get; set; }
        public AttachmentEntityTypeEnum EntityType { get; set; }
        public string EntityTypeTitle
        {
            get
            {
                return EntityType != 0 ? EnumHelper.GetDescription(EntityType) : "";
            }
        }
    }
}
