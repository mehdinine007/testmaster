using IFG.Core.Caching;
using IFG.Core.IOC;
using Microsoft.Extensions.Configuration;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using IFG.Core.Utility.Migration.Domain;

namespace IFG.Core.Utility.Migration
{
    public class DpMigrationsHistory
    {
        private string _connectionStrings;
        private readonly IConfiguration _configuration;
        public DpMigrationsHistory(string connStringName) 
        {
            _configuration = ServiceTool.Resolve<IConfiguration>(); 
            _connectionStrings = _configuration.GetConnectionString(connStringName);

        }
        public IEnumerable<MigrationsHistory> GetList()
        {
            string commandText = " Select Id,MigrationId,Version,StateName,Created from __MigrationsHistory ";
            using (var connection = new SqlConnection(_connectionStrings))
            {
                return connection.Query<MigrationsHistory>(commandText, commandType: CommandType.Text);
            }
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

        public void Add(MigrationsHistory migrationsHistory)
        {
            string commandText =  " insert into __MigrationsHistory (MigrationId,Version,StateName,Created) " +
                                 $" values('{migrationsHistory.MigrationId}','{migrationsHistory.Version}','{migrationsHistory.StateName}',getdate()) ";
            using (var connection = new SqlConnection(_connectionStrings))
            {
                connection.Execute(commandText, commandType: CommandType.Text);
            }
        }

        public void Update(MigrationsHistory migrationsHistory)
        {
            string commandText = "  update __MigrationsHistory " +
                                 $" set MigrationId='{migrationsHistory.MigrationId}',Version='{migrationsHistory.Version}',StateName='{migrationsHistory.StateName}',Created=getdate() " +
                                 $" where id = {migrationsHistory.Id}";
            using (var connection = new SqlConnection(_connectionStrings))
            {
                connection.Execute(commandText, commandType: CommandType.Text);
            }
        }

    }

}
