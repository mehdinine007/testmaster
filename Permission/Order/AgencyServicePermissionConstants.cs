using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Order
{
    public class AgencyServicePermissionConstants
    {
        public const string Delete = ConstantInfo.ModuleOrder + ServiceIdentifier + "0001";
        public const string Delete_DisplayName = "حذف";
        public const string GetAgencies = ConstantInfo.ModuleOrder + ServiceIdentifier + "0002";
        public const string GetAgencies_DisplayName = "نمایش نمایندگی ها";
        public const string Save = ConstantInfo.ModuleOrder + ServiceIdentifier + "0003";
        public const string Save_DisplayName = "ثبت";
        public const string Update = ConstantInfo.ModuleOrder + ServiceIdentifier + "0004";
        public const string Update_DisplayName = "بروزرسانی";

        public const string ServiceIdentifier = "0008";
        public const string ServiceDisplayName = "سرویس نمایندگان";
    }
}
