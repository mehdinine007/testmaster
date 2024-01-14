using Licence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyManagement.Application.Contracts.CompanyManagement.Constants.Permissions
{
    public class BankServicePermissionConstants
    {
        public const string GetAdvocacyUserByCompanyId = ConstantInfo.ModuleIUser + ServiceIdentifier + "0001";
        public const string GetAdvocacyUserByCompanyId_DisplayName = "گرفتن کاربران انصرافی برای شرکت ها";
        public const string GetUserRejecttionAdvocacyList = ConstantInfo.ModuleIUser + ServiceIdentifier + "0002";
        public const string GetUserRejecttionAdvocacyList_DisplayName = "لیست افراد انصراف داده از طرح";
        public const string InquiryAdvocacyUserReport = ConstantInfo.ModuleIUser + ServiceIdentifier + "0003";
        public const string InquiryAdvocacyUserReport_DisplayName = "گزارش حساب های وکالتی";
        public const string InquiryUserRejectionFromBank = ConstantInfo.ModuleIUser + ServiceIdentifier + "0004";
        public const string InquiryUserRejectionFromBank_DisplayName = "لیست افراد انصراف داده از طریق بانک";
        public const string SaveAdvocacyUsersFromBank = ConstantInfo.ModuleIUser + ServiceIdentifier + "0005";
        public const string SaveAdvocacyUsersFromBank_DisplayName = "ثبت حساب وکالتی از طرف بانک";
        public const string SaveUserRejectionFromBank = ConstantInfo.ModuleIUser + ServiceIdentifier + "0006";
        public const string SaveUserRejectionFromBank_DisplayName = "ثبت انصراف از طریق بانک";
        public const string DeleteAdvocayUserFromBank = ConstantInfo.ModuleIUser + ServiceIdentifier + "0007";
        public const string DeleteAdvocayUserFromBank_DisplayName = "ثبت انصراف از طریق بانک";


        public const string ServiceIdentifier = "0002";
        public const string ServiceDisplayName = "سرویس مدیریت بانک";
    }
}
