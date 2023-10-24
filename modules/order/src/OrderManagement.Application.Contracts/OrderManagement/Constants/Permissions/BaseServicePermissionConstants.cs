using Esale.Core.Bases;

namespace OrderManagement.Application.Contracts
{
    public class BaseServicePermissionConstants : BasePermissionConstants
    {
        public const string CheckAdvocacyPrice = "000200010001";
        public const string CheckAdvocacyPrice_DisplayName = "چک حساب وکالتی با مبلغ";
        public const string CheckBlackList = "000200010002";
        public const string CheckBlackList_DisplayName = "چک کردن بلک لیست";
        public const string CheckWhiteList = "000200010003";
        public const string CheckWhiteList_DisplayName = "چک کردن وایت لیست";
        public const string GetAgencies = "000200010004";
        public const string GetAgencies_DisplayName = "نمایش نمایندگی ها";
        public const string ClearCache = "000200010005";
        public const string ClearCache_DisplayName = "پاک کردن کش";

        public override string ModuleIdentifier => "0001";

        public override string ServiceIdentifier => "0001";

        public override string ServiceDisplayName => "سرویس پایه";
    }
}
