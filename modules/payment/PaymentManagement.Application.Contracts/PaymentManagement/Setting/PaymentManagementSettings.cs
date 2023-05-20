namespace PaymentManagement.Application.Contracts.Setting
{
    public static class PaymentManagementSettings
    {
        public const string GroupName = "PaymentManagement";

        /// <summary>
        /// Maximum allowed page size for paged list requests.
        /// </summary>
        public const string MaxPageSize = GroupName + ".MaxPageSize";
    }
}