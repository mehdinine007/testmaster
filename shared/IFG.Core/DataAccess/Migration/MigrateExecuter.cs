using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using IFG.Core.DataAccess.Migration;
using IFG.Core.IOC;

namespace IFG.Core.DataAccess.Migration
{
    public class MigrateExecuter
    {
        private readonly MigrationLog _log;
        private readonly MigrationsHistoryService _migrationsHistoryService;
        private IConfiguration _configuration;
        public MigrateExecuter(string version)
        {
            _log = new MigrationLog();
            _migrationsHistoryService = new MigrationsHistoryService(version);
            _configuration = ServiceTool.Resolve<IConfiguration>();
        }
        public bool Run()
        {
            var connectionString = _configuration["ConnectionStrings:Default"];
            if (string.IsNullOrEmpty(connectionString))
            {
                _log.Write("Configuration file should contain a connection string named 'Default'");
                return false;
            }
            var hostConnStr = new SqlConnectionStringBuilder(connectionString);
            _log.Write("Host database: " + hostConnStr.InitialCatalog);
            _log.Write("Continue to migration for this host database ..? (Y/N): ");
            var command = Console.ReadLine();
            if (command.ToUpper() != "Y")
            {
                _log.Write("Migration canceled.");
                return false;
            }

            _log.Write("HOST database migration started...");
            _log.Write("");

            try
            {
                _migrationsHistoryService.UpdateDatabase();
            }   
            catch (Exception ex)
            {
                _log.Write("An error occured during migration of host database:");
                _log.Write(ex.ToString());
                _log.Write("Canceled migrations.");
                return false;
            }

            _log.Write("");
            _log.Write("HOST database migration completed.");
            _log.Write("--------------------------------------------------------");

            return true;
        }

    }
}
