using Volo.Abp.Settings;

namespace PaymentManagement.Application.Contracts.Setting
{
    /* These setting definitions will be visible to clients that has a ProductManagement.Application.Contracts
     * reference. Settings those should be hidden from clients should be defined in the ProductManagement.Application
     * package.
     */
    public class PaymentManagementSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(
                    PaymentManagementSettings.MaxPageSize,
                    "100",
                    isVisibleToClients: true
                )
            );
        }
    }
}