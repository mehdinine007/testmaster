using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement.Constants.Permissions
{
     public class BankAppServicePermissionConstants : BaseServicePermissionConstants
    {
        public const string GetById = "000200190001";
        public const string GetById_DisplayName = "نمایش بانک ها توسط شناسه";
        public const string GetList = "000200190002";
        public const string GetList_DisplayName = "نمایش بانک ها";
        public const string Delete = "000200190003";
        public const string Delete_DisplayName = "حذف";
        public const string Add = "000200190004";
        public const string Add_DisplayName = "ثبت";
        public const string Update = "000200190005";
        public const string Update_DisplayName = "بروزرسانی";
        public const string UploadFile = "000200190006";
        public const string UploadFile_DisplayName = "بروزرسانی فایل";

        public override string ModuleIdentifier => "0002";

        public override string ServiceIdentifier => "0019";

        public override string ServiceDisplayName => "نمایش بانک ها";
    }


}
