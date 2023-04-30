using Volo.Abp.Settings;

namespace OrderManagement.Application.Contracts
{
    /* These setting definitions will be visible to clients that has a OrderManagement.Application.Contracts
     * reference. Settings those should be hidden from clients should be defined in the OrderManagement.Application
     * package.
     */
    public class OrderManagementSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(
                    OrderManagementSettings.MaxPageSize,
                    "100",
                    isVisibleToClients: true
                )
            );
        }
    }
}