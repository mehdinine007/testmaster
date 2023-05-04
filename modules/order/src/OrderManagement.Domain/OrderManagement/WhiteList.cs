using OrderManagement.Domain.Shared;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class WhiteList : FullAuditedEntity<int>
    {
        public string NationalCode { get; set; }
        public WhiteListEnumType WhiteListType { get; set; }
    }
}
