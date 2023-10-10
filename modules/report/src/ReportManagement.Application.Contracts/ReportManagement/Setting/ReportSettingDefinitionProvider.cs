using ReportManagement.Application.Contracts.Setting;
using Volo.Abp.Settings;

namespace Reportanagement.Application.Contracts.Setting
{
    /* These setting definitions will be visible to clients that has a ProductManagement.Application.Contracts
     * reference. Settings those should be hidden from clients should be defined in the ProductManagement.Application
     * package.
     */
    public class WorkFlowManagementSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                new SettingDefinition(
                    ReportManagementSettings.MaxPageSize,
                    "100",
                    isVisibleToClients: true
                )
            );
        }
    }
}