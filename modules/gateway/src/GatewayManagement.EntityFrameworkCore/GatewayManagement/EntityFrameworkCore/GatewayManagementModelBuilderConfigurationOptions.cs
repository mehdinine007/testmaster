using JetBrains.Annotations;
using GatewayManagement.Domain;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace GatewayManagement.EntityFrameworkCore
{
    public class GatewayManagementModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public GatewayManagementModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = GatewayManagementConsts.DefaultDbTablePrefix,
            [CanBeNull] string schema = GatewayManagementConsts.DefaultDbSchema)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}