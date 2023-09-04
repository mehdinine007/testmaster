namespace UserManagement.Application.Contracts.UserManagement;

public class PermissionDefinitionDto
{
    public string title { get; set; }
    public string nodeCode { get; set; }
    public List<PermissionDefinitionDto> childs { get; set; }
}
