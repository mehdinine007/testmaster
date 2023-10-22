using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using UserManagement.Application.Contracts.Models;
using UserManagement.Application.Contracts.UserManagement.Models.User;

namespace UserManagement.Application.Contracts.UserManagement;

public class PermissionDefinitionDto
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Title { get; set; }
    public string DisplayName { get; set; }
    public string Code { get; set; }
    public List<PermissionDefinitionChildDto> Children { get; set; }
}
