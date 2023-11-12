using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement.Constants.Permissions
{
    public class ProductAndCategoryServicePermissionConstants : BaseServicePermissionConstants
    {
        public const string GetById = "000200160001";
        public const string GetById_DisplayName = "نمایش دسته بندی محصولات توسط شناسه";
        public const string GetList = "000200160002";
        public const string GetList_DisplayName = "نمایش دسته بندی محصولات";
        public const string Delete = "000200160003";
        public const string Delete_DisplayName = "حذف";
        public const string Add = "000200160004";
        public const string Add_DisplayName = "ثبت";
        public const string Update = "000200160005";
        public const string Update_DisplayName = "بروزرسانی";
        public const string UploadFile = "000200160006";
        public const string UploadFile_DisplayName = "بروزرسانی فایل";

        public override string ModuleIdentifier => "0002";

        public override string ServiceIdentifier => "0016";

        public override string ServiceDisplayName => "نمایش دسته بندی محصولات";
    }
}
