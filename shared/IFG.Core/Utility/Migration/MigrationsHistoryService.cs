using IFG.Core.IOC;
using IFG.Core.Utility.Migration.Domain;
using IFG.Core.Utility.Migration.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace IFG.Core.Utility.Migration
{
    public class MigrationsHistoryService 
    {
        private readonly DpMigrationsHistory _migrationsHistoryDal;
        private readonly IConfiguration _configuration;
        public string Version { get; set; }
        public MigrationsHistoryService(string connStringName,string version)
        {
            _migrationsHistoryDal = new DpMigrationsHistory(connStringName);
            Version = version;
            _configuration = ServiceTool.Resolve<IConfiguration>();
        }
        public bool UpdateDatabase()
        {
            string path = Directory.GetCurrentDirectory() + "\\Migrations\\Migrations.json";
            string JsonData = File.ReadAllText(path);
            var migrationData = JsonConvert.DeserializeObject<MigrationData>(JsonData);

            string commandText = "";
            commandText = File.ReadAllText(Directory.GetCurrentDirectory() + "\\Migrations\\dbo.__MigrationsHistory.sql");
            _migrationsHistoryDal.Execute(commandText);

            var migrationsHistory = _migrationsHistoryDal
                .GetList()
                .ToList();

            var fixVersion = Version;
            foreach (var row in migrationData.Schema.OrderBy(x => x.Priority))
            {
                Execute(migrationsHistory, new MigrationsHistory()
                {
                    MigrationId = row.Id,
                    StateName = "Schema",
                    Version = fixVersion,
                    Created = DateTime.Now
                }, row, false);
            }

            foreach (var row in migrationData.Tables.OrderBy(x => x.Priority))
            {
                Execute(migrationsHistory, new MigrationsHistory()
                {
                    MigrationId = row.Id,
                    StateName = "Tables",
                    Version = fixVersion,
                    Created = DateTime.Now
                }, row, false);
            }

            foreach (var row in migrationData.Patch.Where(x => string.Compare(x.Version, fixVersion) <= 0).OrderBy(x => x.Priority))
            {
                if (!migrationsHistory.Any(x => x.MigrationId == row.Id && x.StateName == "Patch"))
                {
                    Execute(migrationsHistory, new MigrationsHistory()
                    {
                        MigrationId = row.Id,
                        StateName = "Patch",
                        Version = fixVersion,
                        Created = DateTime.Now
                    }, row, false);
                }

            }

            foreach (var row in migrationData.Programmability.OrderBy(x => x.Priority))
            {
                Execute(migrationsHistory, new MigrationsHistory()
                {
                    MigrationId = row.Id,
                    StateName = "Programmability",
                    Version = fixVersion,
                    Created = DateTime.Now
                }, row, true);
            }
            var _migrationsHistory = migrationsHistory
            .Where(x => x.MigrationId == "Version")
                .FirstOrDefault();
            if (_migrationsHistory is null)
            {
                _migrationsHistoryDal.Add(new MigrationsHistory()
                {
                    MigrationId = "Version",
                    StateName = "Completed",
                    Version = fixVersion,
                    Created = DateTime.Now
                });
            }
            else
            {
                _migrationsHistory.Version = fixVersion;
                _migrationsHistory.Created = DateTime.Now;
                _migrationsHistoryDal.Update(_migrationsHistory);
            }
            return true;
        }

        private void Execute(List<MigrationsHistory> migrationsHistory, MigrationsHistory data, MigrationItem migrationItem, bool dropObject)
        {
            var _migrationsHistory = migrationsHistory
                .Where(x => x.MigrationId == data.MigrationId && x.StateName == data.StateName)
                .FirstOrDefault();
            if (dropObject)
            {
                _migrationsHistoryDal.Drop(data.MigrationId, migrationItem.Type);
            }
            string commandText = File.ReadAllText(Directory.GetCurrentDirectory() + $"\\Migrations\\{data.StateName}\\{data.MigrationId}.sql");
            var migtationTags = _configuration.GetSection("MigrationTags").Get<MigrationTag>();
            commandText = commandText.Replace(nameof(migtationTags.OrderDb), migtationTags.OrderDb).Replace(nameof(migtationTags.CompanyDb), migtationTags.CompanyDb);
            _migrationsHistoryDal.Execute(commandText);
            if (_migrationsHistory == null)
            {
                _migrationsHistoryDal.Add(data);
            }
            else
            {
                _migrationsHistory.Version = data.Version;
                _migrationsHistory.Created = data.Created;
                _migrationsHistoryDal.Update(_migrationsHistory);
            }
        }

    }
}
