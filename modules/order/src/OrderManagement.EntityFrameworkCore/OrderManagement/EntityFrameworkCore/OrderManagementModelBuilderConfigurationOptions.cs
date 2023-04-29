using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace OrderManagement.EntityFrameworkCore
{
    public class OrderManagementModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public OrderManagementModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = OrderManagementConsts.DefaultDbTablePrefix,
            [CanBeNull] string schema = OrderManagementConsts.DefaultDbSchema)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}