using IFG.Core.Caching;
using IFG.Core.IOC;
using Microsoft.Extensions.Configuration;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using IFG.Core.DataAccess.Migration.Domain;
using IFG.Core.DataAccess.Migration.Models;

namespace IFG.Core.DataAccess.Migration
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

        public IEnumerable<MigrationPermission> GetPermissionList(string objectName)
        {
            string commandText = " select StateDesc = State_desc,PermissionName=Permission_name,UserName = dpr.name " +
                                 " from sys.objects obj " +
                                 "  inner join sys.database_permissions dp ON dp.major_id = obj.object_id " +
                                 "  inner join sys.database_principals dpr on dpr.principal_id = dp.grantee_principal_id " +
                                $" where upper(obj.name) ='{objectName.ToUpper()}'" +
                                 "    and dp.permission_name = 'EXECUTE' " +
                                 "    AND dp.state IN ('G') ";
            using (var connection = new SqlConnection(_connectionStrings))
            {
                return connection.Query<MigrationPermission>(commandText, commandType: CommandType.Text);
            }
        }

        public void ExecutePermission(string userName,string objectName)
        {
            string commandText = $" GRANT EXECUTE ON {objectName} TO {userName}";
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
