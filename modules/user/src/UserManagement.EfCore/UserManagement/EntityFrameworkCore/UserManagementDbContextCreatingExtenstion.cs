using Volo.Abp;
using Microsoft.EntityFrameworkCore;
using UserManagement.EfCore.UserManagement.EntityFrameworkCore;

namespace UserManagement.EfCore.EntityFrameworkCore
{
    public static class UserManagementDbContextCreatingExtenstion
    {
        public static void ConfigureUserManagement(this ModelBuilder builder,
             Action<UserManagementModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new UserManagementModelBuilderConfigurationOptions();

            optionsAction?.Invoke(options);

        }
    }
}
