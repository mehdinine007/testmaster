using IFG.Core.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement.Constants.Permissions
{
    public class ProductPropertyServicePermissionConstants : BasePermissionConstants
    {
        public const string GetById = "000200140001";
        public const string GetById_DisplayName = "نمایش انتصاب محصول توسط شناسه";
        public const string GetList = "000200140002";
        public const string GetList_DisplayName = "نمایش انتصاب محصول";
        public const string Delete = "000200140003";
        public const string Delete_DisplayName = "حذف";
        public const string Add = "000200140004";
        public const string Add_DisplayName = "ثبت";
        public const string Update = "000200140005";
        public const string Update_DisplayName = "بروزرسانی";

        public override string ModuleIdentifier => "0002";

        public override string ServiceIdentifier => "0014";

        public override string ServiceDisplayName => "سرویس انتصاب محصول";
    }
}
