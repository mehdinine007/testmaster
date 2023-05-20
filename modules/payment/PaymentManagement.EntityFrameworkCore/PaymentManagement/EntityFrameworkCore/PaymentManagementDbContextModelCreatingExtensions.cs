using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace PaymentManagement.EntityFrameworkCore
{
    public static class PaymentManagementDbContextModelCreatingExtensions
    {
        public static void ConfigurePaymentManagement(
            this ModelBuilder builder,
            Action<PaymentManagementModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new PaymentManagementModelBuilderConfigurationOptions();

            optionsAction?.Invoke(options);            
        }
    }
}