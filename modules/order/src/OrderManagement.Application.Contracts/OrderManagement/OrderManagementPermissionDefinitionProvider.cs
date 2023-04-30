using OrderManagement.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace OrderManagement.Application.Contracts
{
    public class OrderManagementPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var OrderManagementGroup = context.AddGroup(OrderManagementPermissions.GroupName, L("Permission:OrderManagement"));

            var Orders = OrderManagementGroup.AddPermission(OrderManagementPermissions.Orders.Default, L("Permission:Orders"));
            Orders.AddChild(OrderManagementPermissions.Orders.Update, L("Permission:Edit"));
            Orders.AddChild(OrderManagementPermissions.Orders.Delete, L("Permission:Delete"));
            Orders.AddChild(OrderManagementPermissions.Orders.Create, L("Permission:Create"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<OrderManagementResource>(name);
        }
    }
}