using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using OrderManagement.Localization;
using Volo.Abp.UI.Navigation;

namespace OrderManagement
{
    public class OrderManagementMenuContributor : IMenuContributor
    {
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenu(context);
            }
        }

        private async Task ConfigureMainMenu(MenuConfigurationContext context)
        {
            var l = context.GetLocalizer<OrderManagementResource>();

            var rootMenuItem = new ApplicationMenuItem("OrderManagement", l["Menu:OrderManagement"]);

            if (await context.IsGrantedAsync(OrderManagementPermissions.Orders.Default))
            {
                rootMenuItem.AddItem(new ApplicationMenuItem("Orders", l["Menu:Orders"], "/OrderManagement/Orders"));
            }

            context.Menu.AddItem(rootMenuItem);
        }
    }
}
