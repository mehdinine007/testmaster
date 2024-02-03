using Licence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyManagement.Application.Contracts.CompanyManagement.Constants.Permissions
{
    public class OldCarServicePermissionConstants
    {
        public const string AddList = ConstantInfo.ModuleCompany + ServiceIdentifier + "0001";
        public const string AddList_DisplayName = "افزودن لیست خودروهای فرسوده";
        public const string Inquiry = ConstantInfo.ModuleCompany + ServiceIdentifier + "0002";
        public const string Inquiry_DisplayName = "لیست خودروهای فرسوده";
        public const string Delete = ConstantInfo.ModuleCompany + ServiceIdentifier + "0003";
        public const string Delete_DisplayName = "حذف خودروی فرسوده";
        

        public const string ServiceIdentifier = "0003";
        public const string ServiceDisplayName = "سرویس خودروهای فرسوده";
    }
}
