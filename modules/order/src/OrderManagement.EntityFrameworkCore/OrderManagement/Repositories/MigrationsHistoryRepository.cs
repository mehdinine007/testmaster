using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OrderManagement.Domain;
using OrderManagement.EfCore;
using System.Data;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace OrderManagement.EntityFrameworkCore.OrderManagement.Repositories
{
    public class MigrationsHistoryRepository : EfCoreRepository<OrderManagementDbContext, MigrationsHistory, int>, IMigrationsHistoryRepository
    {
        private readonly IConfiguration _configuration;
        private string _connectionStrings;
        public MigrationsHistoryRepository(IConfiguration configuration, IDbContextProvider<OrderManagementDbContext> dbContextProvider) : base(dbContextProvider)
        {
            _configuration = configuration;
            _connectionStrings = _configuration.GetConnectionString("OrderManagement");

        }
        public void Execute(string commandText)
        {
            using (var connection = new SqlConnection(_connectionStrings))
            {
                connection.Execute(commandText, commandType: CommandType.Text);
            }
        }

        public void Drop(string name, string type)
        {
            string commandText = $" if exists(select 1 from sysobjects where UPPER(name)=UPPER('{name}')) " +
                                 $"  drop {type} {name} ";
            using (var connection = new SqlConnection(_connectionStrings))
            {
                connection.Execute(commandText, commandType: CommandType.Text);
            }
        }


    }

}
