using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement.Constants.Permissions
{
    public class AnnouncementServicePermissionConstants : BaseServicePermissionConstants
    {
        public const string GetById = "000200210001";
        public const string GetById_DisplayName = "نمایش سرویس اطلاعیه توسط شناسه";
        public const string GetList = "000200210002";
        public const string GetList_DisplayName = "نمایش سرویس اطلاعیه";
        public const string Delete = "000200210003";
        public const string Delete_DisplayName = "حذف";
        public const string Add = "000200210004";
        public const string Add_DisplayName = "ثبت";
        public const string Update = "000200210005";
        public const string Update_DisplayName = "بروزرسانی";
        public const string UploadFile = "000200210006";
        public const string UploadFile_DisplayName = "بروزرسانی فایل";

        public override string ModuleIdentifier => "0002";

        public override string ServiceIdentifier => "0021";

        public override string ServiceDisplayName => "نمایش سرویس اطلاعیه";
    }


}
