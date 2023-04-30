using JetBrains.Annotations;
using OrderManagement.Domain;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace OrderManagement.EfCore
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