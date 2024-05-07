using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Order
{
    public class ProductAndCategoryServicePermissionConstants
    {
        public const string GetById = ConstantInfo.ModuleOrder + ServiceIdentifier + "0001";
        public const string GetById_DisplayName = "نمایش دسته بندی محصولات توسط شناسه";
        public const string GetList = ConstantInfo.ModuleOrder + ServiceIdentifier + "0002";
        public const string GetList_DisplayName = "نمایش دسته بندی محصولات";
        public const string Delete = ConstantInfo.ModuleOrder + ServiceIdentifier + "0003";
        public const string Delete_DisplayName = "حذف";
        public const string Add = ConstantInfo.ModuleOrder + ServiceIdentifier + "0004";
        public const string Add_DisplayName = "ثبت";
        public const string Update = ConstantInfo.ModuleOrder + ServiceIdentifier + "0005";
        public const string Update_DisplayName = "بروزرسانی";
        public const string UploadFile = ConstantInfo.ModuleOrder + ServiceIdentifier + "0006";
        public const string UploadFile_DisplayName = "بروزرسانی فایل";
        public const string Move = ConstantInfo.ModuleOrder + ServiceIdentifier + "0007";
        public const string Move_DisplayName = "جابه جایی اولویت ها";
        public const string Import = ConstantInfo.ModuleOrder + ServiceIdentifier + "0008";
        public const string Import_DisplayName = "وارد کردن اکسل مشخصات";

        public const string ServiceIdentifier = "0016";
        public const string ServiceDisplayName = "نمایش دسته بندی محصولات";
    }
}
