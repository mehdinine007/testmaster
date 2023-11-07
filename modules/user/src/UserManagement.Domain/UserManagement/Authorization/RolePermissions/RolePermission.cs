using MongoDB.Bson;
using UserManagement.Domain.UserManagement.Enums;
using Volo.Abp.Domain.Entities.Auditing;

namespace UserManagement.Domain.UserManagement.Authorization.RolePermissions;

public class RolePermission : FullAuditedEntity<ObjectId>
{
    public string Title { get; set; }
    public string Code { get; set; }
    public RolePermissionEnum Type { get; set; }
    public List<string> Permissions { get; set; }
}
