using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.UserManagement.Authorization.RolePermissions;
using UserManagement.Domain.UserManagement.bases;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace UserManagement.EfCore.MongoDb
{
    [ConnectionStringName("MongoConnection")]
    public class UserManagementMongoDbContext : AbpMongoDbContext
    {
        public IMongoCollection<PermissionDefinition> PermissionDefinition => Collection<PermissionDefinition>();
        public IMongoCollection<RolePermission> RolePermission => Collection<RolePermission>();
        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);
        }
    }
}