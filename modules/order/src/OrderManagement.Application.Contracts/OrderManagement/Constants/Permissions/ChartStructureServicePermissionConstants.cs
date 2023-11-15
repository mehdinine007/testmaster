using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement.Constants.Permissions
{
    public class ChartStructureServicePermissionConstants : BaseServicePermissionConstants
    {
        public const string GetById = "000200170001";
        public const string GetById_DisplayName = "نمایش ساختار محصول توسط شناسه";
        public const string GetList = "000200170002";
        public const string GetList_DisplayName = "نمایش ساختار محصول";
        public const string Delete = "000200170003";
        public const string Delete_DisplayName = "حذف";
        public const string Add = "000200170004";
        public const string Add_DisplayName = "ثبت";
        public const string Update = "000200170005";
        public const string Update_DisplayName = "بروزرسانی";
        public const string UploadFile = "000200170006";
        public const string UploadFile_DisplayName = "بروزرسانی فایل";

        public override string ModuleIdentifier => "0002";

        public override string ServiceIdentifier => "0017";

        public override string ServiceDisplayName => "نمایش ساختار محصول";
    }

}
