using Volo.Abp.Reflection;

namespace OrderManagement.Application.Contracts
{
    public class OrderManagementPermissions
    {
        public const string GroupName = "OrderManagement";

        public static class Orders
        {
            public const string Default = GroupName + ".Order";
            public const string Delete = Default + ".Delete";
            public const string Update = Default + ".Update";
            public const string Create = Default + ".Create";

        }
        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(OrderManagementPermissions));
        }
    }
}