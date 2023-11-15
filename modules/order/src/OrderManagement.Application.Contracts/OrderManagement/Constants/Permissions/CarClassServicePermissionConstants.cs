using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement.Constants.Permissions
{
    public class CarClassServicePermissionConstants : BaseServicePermissionConstants
    {
        public const string GetById = "000200180001";
        public const string GetById_DisplayName = "نمایش کلاس ماشین توسط شناسه";
        public const string GetList = "000200180002";
        public const string GetList_DisplayName = "نمایش کلاس ماشین";
        public const string Delete = "000200180003";
        public const string Delete_DisplayName = "حذف";
        public const string Add = "000200180004";
        public const string Add_DisplayName = "ثبت";
        public const string Update = "000200180005";
        public const string Update_DisplayName = "بروزرسانی";
        public const string UploadFile = "000200180006";
        public const string UploadFile_DisplayName = "بروزرسانی فایل";

        public override string ModuleIdentifier => "0002";

        public override string ServiceIdentifier => "0018";

        public override string ServiceDisplayName => "نمایش کلاس ماشین";
    }
}
