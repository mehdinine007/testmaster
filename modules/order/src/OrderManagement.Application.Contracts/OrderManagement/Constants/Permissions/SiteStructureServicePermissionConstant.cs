using IFG.Core.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement.Constants.Permissions
{
    public class SiteStructureServicePermissionConstant: BasePermissionConstants
{
    public const string GetById = "000200120001";
        public const string GetById_DisplayName = "نمایش ساختار سایت توسط شناسه";
        public const string GetList = "000200120002";
        public const string GetList_DisplayName = "نمایش ساختار سایت";
        public const string Delete = "000200120003";
        public const string Delete_DisplayName = "حذف";
        public const string Add = "000200120004";
        public const string Add_DisplayName = "ثبت";
        public const string Update = "000200120005";
        public const string Update_DisplayName = "بروزرسانی";
        public const string UploadFile = "000200120006";
        public const string UploadFile_DisplayName = "بروزرسانی فایل";

        public override string ModuleIdentifier => "0002";

        public override string ServiceIdentifier => "0012";

        public override string ServiceDisplayName => "سرویس ساختار سایت";
    }
}
