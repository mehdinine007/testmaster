namespace GatewayManagement.Application.Contracts.Setting
{
    public static class GatewayManagementSettings
    {
        public const string GroupName = "GatewayManagement";

        /// <summary>
        /// Maximum allowed page size for paged list requests.
        /// </summary>
        public const string MaxPageSize = GroupName + ".MaxPageSize";
    }
}