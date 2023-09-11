using UserManagement.Domain.UserManagement.Authorization.RolePermissions;

namespace UserManagement.Application.Contracts.Models;

public class RolePermissionDataDto
{
    public string Title { get; set; }
    public string Code { get; set; }
}
