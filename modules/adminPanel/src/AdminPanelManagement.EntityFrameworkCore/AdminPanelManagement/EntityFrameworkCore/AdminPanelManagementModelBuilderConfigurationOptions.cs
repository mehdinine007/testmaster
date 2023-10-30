using JetBrains.Annotations;
using AdminPanelManagement.Domain;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace AdminPanelManagement.EntityFrameworkCore
{
    public class AdminPanelManagementModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public AdminPanelManagementModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = AdminPanelManagementConsts.DefaultDbTablePrefix,
            [CanBeNull] string schema = AdminPanelManagementConsts.DefaultDbSchema)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}