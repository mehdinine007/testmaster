namespace UserManagement.Application.Contracts.Models;

public class MenuChildDto
{
    public string Title { get; set; }
    public string Code { get; set; }
    public int Type { get; set; }
    public string Icon { get; set; }
    public string Url { get; set; }
    public List<MenuChildDto> Children { get; set; }
}
