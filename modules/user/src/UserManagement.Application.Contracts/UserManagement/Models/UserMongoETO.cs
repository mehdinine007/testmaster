using Abp.Authorization.Users;
using Castle.MicroKernel.SubSystems.Conversion;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.Authorization.Users;
using UserManagement.Domain.UserManagement.Enums;
using Volo.Abp.EventBus;

namespace UserManagement.Application.Contracts.Models
{
    [EventName("UserMongo")]
    public class UserMongoETO: UserMongo
    {
        //  public string MongoId { get;set; }
        //    [BsonId]
        //    [BsonRepresentation(BsonType.ObjectId)]
        //    public string _Id { get; set; }
        public string MongoId { get; set; }
    }
}
