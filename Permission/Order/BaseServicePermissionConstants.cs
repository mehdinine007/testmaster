using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Order
{
    public class BaseServicePermissionConstants
    {
        public const string CheckAdvocacyPrice = ConstantInfo.ModuleOrder + ServiceIdentifier + "0001";
        public const string CheckAdvocacyPrice_DisplayName = "چک حساب وکالتی با مبلغ";
        public const string CheckBlackList = ConstantInfo.ModuleOrder + ServiceIdentifier + "0002";
        public const string CheckBlackList_DisplayName = "چک کردن بلک لیست";
        public const string CheckWhiteList = ConstantInfo.ModuleOrder + ServiceIdentifier + "0003";
        public const string CheckWhiteList_DisplayName = "چک کردن وایت لیست";
        public const string GetAgencies = ConstantInfo.ModuleOrder + ServiceIdentifier + "0004";
        public const string GetAgencies_DisplayName = "نمایش نمایندگی ها";
        public const string ClearCache = ConstantInfo.ModuleOrder + ServiceIdentifier + "0005";
        public const string ClearCache_DisplayName = "پاک کردن کش";

        public const string ServiceIdentifier = "0001";
        public const string ServiceDisplayName = "سرویس پایه";
    }
}
