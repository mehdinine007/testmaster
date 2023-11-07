using Volo.Abp.Reflection;

namespace WorkFlowManagement.Application.Contracts
{
    public class ReportManagementPermissions
    {
        public const string GroupName = "WorkFlowManagement";

        public static class Products
        {
            public const string Default = GroupName + ".Product";
            public const string Delete = Default + ".Delete";
            public const string Update = Default + ".Update";
            public const string Create = Default + ".Create";

        }
        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(ReportManagementPermissions));
        }
    }
}