using JetBrains.Annotations;
using PaymentManagement.Domain;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace PaymentManagement.EntityFrameworkCore
{
    public class PaymentManagementModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public PaymentManagementModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = PaymentManagementConsts.DefaultDbTablePrefix,
            [CanBeNull] string schema = PaymentManagementConsts.DefaultDbSchema)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}