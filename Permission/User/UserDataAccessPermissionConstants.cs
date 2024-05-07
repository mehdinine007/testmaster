using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.User
{
    public class UserDataAccessPermissionConstants
    {
        public const string GetListByNationalcode = ConstantInfo.ModuleIUser + ServiceIdentifier + "0001";
        public const string GetListByNationalcode_DisplayName = "نمایش دسترسی  توسط کدملی";
        public const string GetListByUserId = ConstantInfo.ModuleIUser + ServiceIdentifier + "0002";
        public const string GetListByUserId_DisplayName = "نمایش بانک ها توسط شناسه";


        public const string ServiceIdentifier = "0005";
        public const string ServiceDisplayName = "سرویس مجوز نقش ها";
    }
}
