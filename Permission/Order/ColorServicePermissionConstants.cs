using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Order
{
    public class ColorServicePermissionConstants
    {
        public const string Delete = ConstantInfo.ModuleOrder + ServiceIdentifier + "0001";
        public const string Delet_DisplayName = "حذف";
        public const string GetAllColors = ConstantInfo.ModuleOrder + ServiceIdentifier + "0002";
        public const string GetAllColors_DisplayName = "نمایش همه رنگ ها";
        public const string GetColors = ConstantInfo.ModuleOrder + ServiceIdentifier + "0003";
        public const string GetColors_DisplayName = "نمایش رنگ";
        public const string Save = ConstantInfo.ModuleOrder + ServiceIdentifier + "0004";
        public const string Save_DisplayName = "ثبت";
        public const string Update = ConstantInfo.ModuleOrder + ServiceIdentifier + "0005";
        public const string Update_DisplayName = "بروزرسانی";

        public const string ServiceIdentifier = "0010";
        public const string ServiceDisplayName = "سرویس مدیریت رنگ";
    }
}
