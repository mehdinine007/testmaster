using MongoDB.Driver;
using OrderManagement.Domain;
using OrderManagement.Domain.OrderManagement.MongoWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace OrderManagement.EfCore.MongoDb
{
    [ConnectionStringName("MongoWriteConnection")]
    public class OrderManagementMongoDbContextWriteOnly : AbpMongoDbContext
    {
        public IMongoCollection<PropertyCategoryWrite> PropertyCategories => Collection<PropertyCategoryWrite>();
        public IMongoCollection<ProductPropertyWrite> ProductProperties => Collection<ProductPropertyWrite>();
        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);
        }
    }
}