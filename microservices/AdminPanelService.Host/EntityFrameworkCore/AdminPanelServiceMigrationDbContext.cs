using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using AdminPanelManagement.EntityFrameworkCore;

namespace AdminPanelService.Host.EntityFrameworkCore
{
    public class AdminPanelServiceMigrationDbContext : AbpDbContext<AdminPanelServiceMigrationDbContext>
    {
        public AdminPanelServiceMigrationDbContext(
            DbContextOptions<AdminPanelServiceMigrationDbContext> options
            ) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureAdminPanelManagement();
        }
    }
}
