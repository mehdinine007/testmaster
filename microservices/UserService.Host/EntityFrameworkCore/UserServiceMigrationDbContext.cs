#region NS
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
#endregion


namespace UserService.Host.EntityFrameworkCore
{
    public class UserServiceMigrationDbContext : AbpDbContext<UserServiceMigrationDbContext>
    {
        public UserServiceMigrationDbContext(
           DbContextOptions<UserServiceMigrationDbContext> options
           ) : base(options)
        {

        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //TODO:
        }
    }
}
