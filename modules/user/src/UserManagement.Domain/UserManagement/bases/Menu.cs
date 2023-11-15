using MongoDB.Bson;
using Volo.Abp.Domain.Entities.Auditing;

namespace UserManagement.Domain.UserManagement.bases;

public class Menu : FullAuditedEntity<ObjectId>
{
    public string Title { get; set; }
    public string Code { get; set; }
    public int Type { get; set; }
    public string  Icon { get; set; }
    public string Url { get; set; }
    public List<MenuChild> Children { get; set; }

    public List<PermissionDefinitionChild> Permissions { get; set; }



}
