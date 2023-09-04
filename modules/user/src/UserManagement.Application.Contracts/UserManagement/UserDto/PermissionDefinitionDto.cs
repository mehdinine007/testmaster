using UserManagement.Application.Contracts.UserManagement.UserDto;

namespace UserManagement.Application.Contracts.UserManagement;

public class PermissionDefinitionDto
{
    public string Title { get; set; }
    public string Code { get; set; }
    public List<PermissionDefinitionChildDto> Children { get; set; }
}
