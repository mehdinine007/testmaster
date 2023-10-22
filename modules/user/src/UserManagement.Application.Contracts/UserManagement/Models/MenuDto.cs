using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using UserManagement.Application.Contracts.Models;
using UserManagement.Application.Contracts.UserManagement.Models.User;

namespace UserManagement.Application.Contracts.UserManagement;

public class MenuDto
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Title { get; set; }
    public string Code { get; set; }
    public int Type { get; set; }
    public string Icon { get; set; }
    public string Url { get; set; }
    public List<MenuChildDto> Children { get; set; }
}
