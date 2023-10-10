using JetBrains.Annotations;
using ReportManagement.Domain.ReportManagement;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace ReportManagement.EntityFrameworkCore
{
    public class ReportManagementModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public ReportManagementModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = ReportManagementConsts.DefaultDbTablePrefix,
            [CanBeNull] string schema = ReportManagementConsts.DefaultDbSchema)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}