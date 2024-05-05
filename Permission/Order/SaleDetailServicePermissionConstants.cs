using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Order
{
    public class SaleDetailServicePermissionConstants
    {
        public const string Delete = ConstantInfo.ModuleOrder + ServiceIdentifier + "0001";
        public const string Delete_DisplayName = "حذف";
        public const string GetActiveList = ConstantInfo.ModuleOrder + ServiceIdentifier + "0002";
        public const string GetActiveList_DisplayName = "نمایش برنامه فروش های فعال";
        public const string GetById = ConstantInfo.ModuleOrder + ServiceIdentifier + "0003";
        public const string GetById_DisplayName = "نمایش برنامه فروش توسط شناسه";
        public const string GetSaleDetails = ConstantInfo.ModuleOrder + ServiceIdentifier + "0004";
        public const string GetSaleDetails_DisplayName = "نمایش برنامه فروش ها";
        public const string Save = ConstantInfo.ModuleOrder + ServiceIdentifier + "0005";
        public const string Save_DisplayName = "ثبت";
        public const string Update = ConstantInfo.ModuleOrder + ServiceIdentifier + "0006";
        public const string Update_DisplayName = "بروزرسانی";

        public const string ServiceIdentifier = "0011";
        public const string ServiceDisplayName = "سرویس مدیریت برنامه فروش";
    }
}
