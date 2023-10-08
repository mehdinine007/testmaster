using JetBrains.Annotations;
using OrderManagement.Domain;
using OrderManagement.Domain.CompanyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace OrderManagement.EfCore.CompanyManagement.EntityFrameworkCore
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
