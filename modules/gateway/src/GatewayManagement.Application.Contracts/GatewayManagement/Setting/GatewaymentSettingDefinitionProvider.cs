using Volo.Abp.Settings;

namespace GatewayManagement.Application.Contracts.Setting
{
    /* These setting definitions will be visible to clients that has a ProductManagement.Application.Contracts
     * reference. Settings those should be hidden from clients should be defined in the ProductManagement.Application
     * package.
     */
    public class GatewayManagementSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(
                    GatewayManagementSettings.MaxPageSize,
                    "100",
                    isVisibleToClients: true
                )
            );
        }
    }
}