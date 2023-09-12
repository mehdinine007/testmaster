namespace UserManagement.Application.Contracts.Models;

public class PermissionDefinitionDto
{
    public string Title { get; set; }
    public string Code { get; set; }
    public List<PermissionDefinitionChildDto> Children { get; set; }
}
