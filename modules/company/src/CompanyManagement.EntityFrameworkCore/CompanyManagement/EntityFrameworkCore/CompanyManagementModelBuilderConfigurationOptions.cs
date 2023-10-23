using CompanyManagement.Domain.CompanyManagement;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace CompanyManagement.EfCore.CompanyManagement.EntityFrameworkCore
{
    public class CompanyManagementModelBuilderConfigurationOptions: AbpModelBuilderConfigurationOptions
    {
        public CompanyManagementModelBuilderConfigurationOptions(
             [NotNull] string tablePrefix = CompanyManagementConsts.DefaultDbTablePrefix,
             [CanBeNull] string schema = CompanyManagementConsts.DefaultDbSchema)
             : base(
                 tablePrefix,
                 schema)
        {

        }

    }
}
