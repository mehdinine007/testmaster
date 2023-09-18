using MongoDB.Bson;
using Volo.Abp.Domain.Entities.Auditing;

namespace UserManagement.Domain.UserManagement.bases
{
    public class PermissionDefinition : FullAuditedEntity<ObjectId>
    {
        public string Title { get; set; }
        public string Code { get; set; }
        public List<PermissionDefinitionChild> Children { get; set; }
    }
}
