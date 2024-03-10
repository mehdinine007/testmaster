using Licence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyManagement.Application.Contracts.CompanyManagement.Constants.Permissions
{
    public class ClientOrderDetailCompanyPermission
    {
        public const string Save = ConstantInfo.ModuleCompany + ServiceIdentifier + "0001";
        public const string Save_DisplayName = "ثبت سفارش خودروساز";
        public const string GetList = ConstantInfo.ModuleCompany + ServiceIdentifier + "0002";
        public const string GetList_DisplayName = " نمایش  سفارش خودروساز";


        public const string ServiceIdentifier = "0004";
        public const string ServiceDisplayName = "سرویس شرکت ها";
    }
}
