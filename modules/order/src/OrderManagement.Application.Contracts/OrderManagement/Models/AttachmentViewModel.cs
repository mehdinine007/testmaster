using Esale.Core.Utility.Tools;
using Newtonsoft.Json;
using OrderManagement.Domain.Shared;

namespace OrderManagement.Application.Contracts
{
    public class AttachmentViewModel
    {
        [JsonIgnore]
        public string Id { get; set; }
        [JsonIgnore]
        public string FileExtension { get; set; }
        public string Title { get; set; }
        public string FileName {
            get
            {
                return Id.ToString() + "." + FileExtension;
            }
        }
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
