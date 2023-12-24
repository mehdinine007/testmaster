using CompanyManagement.Domain.CompanyManagement;
using MongoDB.Driver;
using System;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace CompanyManagement.EfCore.CompanyManagement.MongoDb
{
    [Obsolete]
    [ConnectionStringName("MongoConnection")]
    public class CompanyManagementMongoDbContext : AbpMongoDbContext
    {
        public IMongoCollection<UserMongo> Customers => Collection<UserMongo>();
    }
}
