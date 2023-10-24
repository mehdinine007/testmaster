namespace UserManagement.Application.Contracts.Models;

public class PermissionDefinitionChildDto
{
    public string Title { get; set; }
    public string Code { get; set; }
    public string DisplayName { get; set; }
    public List<PermissionDefinitionChildDto> Children { get; set; }
}
