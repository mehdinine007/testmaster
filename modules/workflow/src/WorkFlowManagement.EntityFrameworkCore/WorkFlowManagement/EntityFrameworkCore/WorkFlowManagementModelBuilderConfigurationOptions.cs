using JetBrains.Annotations;
using WorkFlowManagement.Domain;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace WorkFlowManagement.EntityFrameworkCore
{
    public class WorkFlowManagementModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public WorkFlowManagementModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = WorkFlowManagementConsts.DefaultDbTablePrefix,
            [CanBeNull] string schema = WorkFlowManagementConsts.DefaultDbSchema)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}