using MongoDB.Driver;
using OrderManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.UserManagement.bases;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace UserManagement.EfCore.MongoDb
{
    [ConnectionStringName("MongoConnection")]
    public class UserManagementMongoDbContext : AbpMongoDbContext
    {
        public IMongoCollection<PermissionDefinition> PermissionDefinition => Collection<PermissionDefinition>();
        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);
        }
    }
}