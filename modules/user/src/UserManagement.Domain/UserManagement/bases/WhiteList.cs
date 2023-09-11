using Volo.Abp.Domain.Entities.Auditing;

namespace UserManagement.Domain.UserManagement.Bases
{
    public class WhiteList : FullAuditedEntity<int>
    {
        public string NationalCode { get; set; }
        public  WhiteListEnumType WhiteListType { get; set; }
    }
    
    public enum WhiteListEnumType
    {
        WhiteListBeforeLogin = 1,
        WhiteListOrder = 2,
        WhilteListEnseraf = 3
    }
}
