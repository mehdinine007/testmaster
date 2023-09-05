using MongoDB.Bson;
using Volo.Abp.Domain.Entities.Auditing;

namespace UserManagement.Domain.UserManagement.Authorization.RolePermissions
{
    public class PermissionDataDto
    {
        public string Title { get; set; }
        public string Code { get; set; }
    }
}
