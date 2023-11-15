using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement.Constants.Permissions
{
    public class AttachmentServicePermissionConstants : BaseServicePermissionConstants
    {
        public const string GetById = "000200200001";
        public const string GetById_DisplayName = "نمایش سرویس پیوست توسط شناسه";
        public const string GetList = "000200200002";
        public const string GetList_DisplayName = "نمایش سرویس پیوست";
        public const string Delete = "000200200003";
        public const string Delete_DisplayName = "حذف";
        public const string Add = "000200200004";
        public const string Add_DisplayName = "ثبت";
        public const string Update = "000200200005";
        public const string Update_DisplayName = "بروزرسانی";
        public const string UploadFile = "000200200006";
        public const string UploadFile_DisplayName = "بروزرسانی فایل";

        public override string ModuleIdentifier => "0002";

        public override string ServiceIdentifier => "0020";

        public override string ServiceDisplayName => "نمایش سرویس پیوست";
    }


}
