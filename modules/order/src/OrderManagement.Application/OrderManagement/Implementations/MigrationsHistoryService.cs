using Licence;
using Newtonsoft.Json;
using OrderManagement.Application.Contracts;
using OrderManagement.Application.Contracts.OrderManagement.Services;
using OrderManagement.Domain;
using OrderManagement.EntityFrameworkCore.OrderManagement.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace OrderManagement.Application.OrderManagement.Implementations
{
    public class MigrationsHistoryService : ApplicationService, IMigrationsHistoryService
    {
        private readonly IRepository<MigrationsHistory, int> _migrationsHistoryRepository;
        private readonly IMigrationsHistoryRepository _migrationsHistoryDal;
        public MigrationsHistoryService(IRepository<MigrationsHistory, int> migrationsHistoryRepository, IMigrationsHistoryRepository migrationsHistoryDal)
        {
            _migrationsHistoryRepository = migrationsHistoryRepository;
            _migrationsHistoryDal = migrationsHistoryDal;
        }
        public async Task<bool> UpdateDatabase()
        {
            string path = Directory.GetCurrentDirectory() + "\\Migrations\\Migrations.json";
            string JsonData = File.ReadAllText(path);
            var migrationData = JsonConvert.DeserializeObject<MigrationData>(JsonData);

            string commandText = "";
            commandText = File.ReadAllText(Directory.GetCurrentDirectory() + "\\Migrations\\dbo.__MigrationsHistory.sql");
            _migrationsHistoryDal.Execute(commandText);

            var migrationsHistory = 
                (await _migrationsHistoryRepository.GetQueryableAsync())
            .ToList();

            var fixVersion = AppLicence.GetVersion("").FixVersion;
            foreach (var row in migrationData.Schema.OrderBy(x => x.Priority))
            {
                await Execute(migrationsHistory, new MigrationsHistory()
                {
                    MigrationId = row.Id,
                    StateName = "Schema",
                    Version = fixVersion,
                    Created = DateTime.Now
                }, row, false);
            }

            foreach (var row in migrationData.Tables.OrderBy(x => x.Priority))
            {
                await Execute(migrationsHistory, new MigrationsHistory()
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
                    await Execute(migrationsHistory, new MigrationsHistory()
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
                await Execute(migrationsHistory, new MigrationsHistory()
                {
                    MigrationId = row.Id,
                    StateName = "Programmability",
                    Version = fixVersion,
                    Created = DateTime.Now
                }, row, true);
            }

            await _migrationsHistoryRepository.InsertAsync(new MigrationsHistory()
            {
                MigrationId = "Version",
                StateName = "Completed",
                Version = fixVersion,
                Created = DateTime.Now
            },autoSave:true);
            return true;
        }

        private async Task Execute(List<MigrationsHistory> migrationsHistory, MigrationsHistory data, MigrationItem migrationItem, bool dropObject)
        {
            var _migrationsHistory = migrationsHistory
                .Where(x => x.MigrationId == data.MigrationId && x.StateName == data.StateName)
                .FirstOrDefault();
            if (dropObject)
            {
                _migrationsHistoryDal.Drop(data.MigrationId, migrationItem.Type);
            }
            string commandText = File.ReadAllText(Directory.GetCurrentDirectory() + $"\\Migrations\\{data.StateName}\\{data.MigrationId}.sql");
            _migrationsHistoryDal.Execute(commandText);
            if (_migrationsHistory == null)
            {
                await _migrationsHistoryRepository.InsertAsync(data,autoSave:true);
            }
            else
            {
                _migrationsHistory.Version = data.Version;
                _migrationsHistory.Created = data.Created;
                await _migrationsHistoryRepository.UpdateAsync(_migrationsHistory,autoSave:true);
            }
        }

    }
}
