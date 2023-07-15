using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace GatewayManagement.EntityFrameworkCore
{
    public static class GatewayManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureGatewayManagement(
            this ModelBuilder builder,
            Action<GatewayManagementModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new GatewayManagementModelBuilderConfigurationOptions();

            optionsAction?.Invoke(options);            
        }
    }
}