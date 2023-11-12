using IFG.Core.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement.Constants.Permissions
{
    public class PropertyCategoryServicePermissionConstants : BasePermissionConstants
    {
        public const string GetById = "000200130001";
        public const string GetById_DisplayName = "نمایش انتصاب محصول به ویژگی با شناسه";
        public const string GetList = "000200130002";
        public const string GetList_DisplayName = "نمایش انتصاب محصول به ویژگی";
        public const string Delete = "000200130003";
        public const string Delete_DisplayName = "حذف";
        public const string Add = "000200130004";
        public const string Add_DisplayName = "ثبت";
        public const string Update = "000200130005";
        public const string Update_DisplayName = "بروزرسانی";
        public const string UploadFile = "000200130006";
        public const string UploadFile_DisplayName = "بروزرسانی فایل";

        public override string ModuleIdentifier => "0002";

        public override string ServiceIdentifier => "0013";

        public override string ServiceDisplayName => "سرویس انتصاب محصول به ویژگی";
    }
}
