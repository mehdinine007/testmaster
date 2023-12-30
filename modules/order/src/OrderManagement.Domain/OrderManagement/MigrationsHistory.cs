using System;
using Volo.Abp.Domain.Entities;

namespace OrderManagement.Domain
{
    public class MigrationsHistory : Entity<int>
    {
        public int Id { get; set; }
        public string MigrationId { get; set; }
        public string Version { get; set; }
        public string StateName { get; set; }
        public DateTime Created { get; set; }
    }
}
