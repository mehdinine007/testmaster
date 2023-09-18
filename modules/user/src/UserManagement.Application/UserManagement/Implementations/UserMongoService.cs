
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;
using UserManagement.Domain.Authorization.Users;

namespace WorkingWithMongoDB.WebAPI.Services
{
    public class MyMongoDBDateTimeSerializer : DateTimeSerializer
    {
        //  MongoDB returns datetime as DateTimeKind.Utc, which cann't be used in our timezone conversion logic
        //  We overwrite it to be DateTimeKind.Unspecified
        public override DateTime Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var obj = base.Deserialize(context, args);
            return new DateTime(obj.Ticks, DateTimeKind.Unspecified);
        }

        //  MongoDB stores all datetime as Utc, any datetime value DateTimeKind is not DateTimeKind.Utc, will be converted to Utc first
        //  We overwrite it to be DateTimeKind.Utc, becasue we want to preserve the raw value
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateTime value)
        {
            var utcValue = new DateTime(value.Ticks, DateTimeKind.Utc);
            base.Serialize(context, args, utcValue);
        }

    }
    public class MyMongoDBGuidSerializer : GuidSerializer
    {
        //  MongoDB returns datetime as DateTimeKind.Utc, which cann't be used in our timezone conversion logic
        //  We overwrite it to be DateTimeKind.Unspecified
        public override Guid Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var obj = base.Deserialize(context, args);
            return Guid.NewGuid();
            // return new DateTime(obj.Ticks, DateTimeKind.Unspecified);
        }

        //  MongoDB stores all datetime as Utc, any datetime value DateTimeKind is not DateTimeKind.Utc, will be converted to Utc first
        //  We overwrite it to be DateTimeKind.Utc, becasue we want to preserve the raw value
        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Guid value)
        {
            //var utcValue = new DateTime(value.Ticks, DateTimeKind.Utc);
            base.Serialize(context, args, Guid.NewGuid());
        }


    }
    public class UserMongoService
    {
        private readonly IMongoCollection<UserMongo> _customer;

        private IConfiguration _configuration { get; set; }


        public UserMongoService(IConfiguration Configuration)
        {

            _configuration = Configuration;
            var client = new MongoClient(_configuration.GetConnectionString("MongoConnection"));
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.CSharpLegacy));
            var d = _configuration.GetConnectionString("MongoConnection").Split("/").Last();
            var database = client.GetDatabase(_configuration.GetConnectionString("MongoConnection").Split("/").Last());

            try
            {
                _customer = database.GetCollection<UserMongo>(_configuration.GetSection("MongoConfiguration:UserCollection").Value);

                BsonSerializer.RegisterSerializer(typeof(DateTime), new MyMongoDBDateTimeSerializer());
                //  BsonSerializer.RegisterSerializer(typeof(Guid), new MyMongoDBDateTimeSerializer());

                //BsonSerializer.RegisterSerializer(DateTimeSerializer.LocalInstance);

            }
            catch (Exception ex)
            {

            }

        }
        public async Task<IMongoCollection<UserMongo>> GetUserCollection()
        {
            return _customer;
        }
        public IMongoCollection<UserMongo> GetUserCollectionSync()
        {
            return _customer;
        }



    }
}
