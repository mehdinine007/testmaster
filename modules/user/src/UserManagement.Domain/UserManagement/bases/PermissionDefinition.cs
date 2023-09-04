using MongoDB.Bson;
using Volo.Abp.Domain.Entities.Auditing;

namespace UserManagement.Domain.UserManagement.bases
{
    public class PermissionDefinition : FullAuditedEntity<ObjectId>
    {
        public string title { get; set; }
        public string nodeCode { get; set; }
        public List<PermissionDefinition> childs { get; set; }
    }
}
