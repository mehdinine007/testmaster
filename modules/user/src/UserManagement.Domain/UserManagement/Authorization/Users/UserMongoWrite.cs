using MongoDB.Bson.Serialization.Attributes;

namespace UserManagement.Domain.Authorization.Users;

[BsonIgnoreExtraElements]
public class UserMongoWrite : UserMongo
{

}