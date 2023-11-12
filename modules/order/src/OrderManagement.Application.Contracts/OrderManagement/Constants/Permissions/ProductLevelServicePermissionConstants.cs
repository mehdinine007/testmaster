using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement.Constants.Permissions
{
    public class ProductLevelServicePermissionConstants : BaseServicePermissionConstants
    {
        public const string GetById = "000200150001";
        public const string GetById_DisplayName = "نمایش سطح بندی محصول توسط شناسه";
        public const string GetList = "000200150002";
        public const string GetList_DisplayName = "نمایش سطح بندی محصول";
        public const string Delete = "000200150003";
        public const string Delete_DisplayName = "حذف";
        public const string Add = "000200150004";
        public const string Add_DisplayName = "ثبت";
        public const string Update = "000200150005";
        public const string Update_DisplayName = "بروزرسانی";

        public override string ModuleIdentifier => "0002";

        public override string ServiceIdentifier => "0015";

        public override string ServiceDisplayName => "سرویس سطح بندی محصول";
    }
}
