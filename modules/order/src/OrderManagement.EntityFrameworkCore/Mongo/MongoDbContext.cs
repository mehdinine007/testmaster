using MongoDB.Driver;
using OrderManagement.Mongo.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace OrderManagement.EfCore.Mongo
{
    [ConnectionStringName("MongoConnection")]
    public class MongoDbContext : AbpMongoDbContext
    {
        public IMongoCollection<UserMongo> Customers => Collection<UserMongo>();
        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            //Customize the configuration for your collections.
            //modelBuilder.Entity<Question>(b =>
            //{
            //    b.CollectionName = "MyQuestions"; //Sets the collection name
            //    b.BsonMap.UnmapProperty(x => x.MyProperty); //Ignores 'MyProperty'
            //});
        }
    }
}