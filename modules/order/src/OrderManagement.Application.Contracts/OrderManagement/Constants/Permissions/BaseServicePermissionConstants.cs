using Esale.Core.Bases;

namespace OrderManagement.Application.Contracts
{
    public class BaseServicePermissionConstants : BasePermissionConstants
    {
        public const string CheckAdvocacyPrice = "000100010001";
        public const string CheckBlackList = "000100010002";
        public const string CheckWhiteList = "000100010003";
        public const string GetAgencies = "000100010004";
        public const string ClearCache = "000100010005";

    }
}
