using System;
using Volo.Abp.Domain.Entities;

namespace IFG.Core.Utility.Migration.Domain
{
    public class MigrationsHistory 
    {
        public int Id { get; set; }
        public string MigrationId { get; set; }
        public string Version { get; set; }
        public string StateName { get; set; }
        public DateTime Created { get; set; }
    }
}
