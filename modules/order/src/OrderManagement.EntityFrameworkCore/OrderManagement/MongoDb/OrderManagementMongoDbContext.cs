using MongoDB.Driver;
using OrderManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace OrderManagement.EfCore.MongoDb
{
    [ConnectionStringName("MongoConnection")]
    public class OrderManagementMongoDbContext : AbpMongoDbContext
    {
        public IMongoCollection<PropertyCategory> PropertyCategories => Collection<PropertyCategory>();
        public IMongoCollection<ProductProperty> ProductProperties => Collection<ProductProperty>();
        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);
        }
    }
}