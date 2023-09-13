using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using UserManagement.Domain.UserManagement.Authorization.RolePermissions;

namespace UserManagement.Application.Contracts.Models;

public class RolePermissionDto
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public string Title { get; set; }
    public string Code { get; set; }
    public List<string> Permissions { get; set; }
}
