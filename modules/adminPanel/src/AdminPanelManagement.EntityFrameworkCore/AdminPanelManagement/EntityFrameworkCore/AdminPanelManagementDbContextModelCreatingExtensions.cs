
using Microsoft.EntityFrameworkCore;
using Volo.Abp;

using Volo.Abp.EntityFrameworkCore.Modeling;
using System.Reflection.Emit;
using AdminPanelManagement.EntityFrameworkCore;

namespace AdminPanelManagement.EntityFrameworkCore
{
    public static class AdminPanelManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureAdminPanelManagement(
            this ModelBuilder builder,
            Action<AdminPanelManagementModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new AdminPanelManagementModelBuilderConfigurationOptions();

            optionsAction?.Invoke(options);

          

        }
    }
}