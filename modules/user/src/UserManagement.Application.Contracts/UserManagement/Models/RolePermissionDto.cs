using UserManagement.Domain.UserManagement.Authorization.RolePermissions;

namespace UserManagement.Application.Contracts.Models;

public class RolePermissionDto
{
    public string Title { get; set; }
    public string Code { get; set; }
    public List<PermissionDataDto> Permissions { get; set; }
}
