namespace ReportManagement.Application.Contracts.Setting
{
    public static class ReportManagementSettings
    {
        public const string GroupName = "ReportManagement";

        /// <summary>
        /// Maximum allowed page size for paged list requests.
        /// </summary>
        public const string MaxPageSize = GroupName + ".MaxPageSize";
    }
}