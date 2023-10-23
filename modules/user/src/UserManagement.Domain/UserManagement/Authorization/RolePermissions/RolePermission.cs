using MongoDB.Bson;
using Volo.Abp.Domain.Entities.Auditing;

namespace UserManagement.Domain.UserManagement.Authorization.RolePermissions;

public class RolePermission : FullAuditedEntity<ObjectId>
{
    public string Title { get; set; }
    public string Code { get; set; }

    public List<string> Permissions { get; set; }
}
