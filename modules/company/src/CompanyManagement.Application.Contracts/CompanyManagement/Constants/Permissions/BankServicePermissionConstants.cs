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
       
        public const string InquiryUserRejectionFromBank = ConstantInfo.ModuleCompany + ServiceIdentifier + "0001";
        public const string InquiryUserRejectionFromBank_DisplayName = "لیست افراد انصراف داده از طریق بانک";
        public const string SaveAdvocacyUsersFromBank = ConstantInfo.ModuleCompany + ServiceIdentifier + "0002";
        public const string SaveAdvocacyUsersFromBank_DisplayName = "ثبت حساب وکالتی از طرف بانک";
        public const string SaveUserRejectionFromBank = ConstantInfo.ModuleCompany + ServiceIdentifier + "0003";
        public const string SaveUserRejectionFromBank_DisplayName = "ثبت انصراف از طریق بانک";
        public const string DeleteAdvocayUserFromBank = ConstantInfo.ModuleCompany + ServiceIdentifier + "0004";
        public const string DeleteAdvocayUserFromBank_DisplayName = "حذف حساب وکالتی از طریق بانک";
        public const string InquiryAdvocacyUsersFromBank = ConstantInfo.ModuleCompany + ServiceIdentifier + "0005";
        public const string InquiryAdvocacyUsersFromBank_DisplayName = "حساب وکالتی ثبت شده توسط بانک";

        

        public const string ServiceIdentifier = "0002";
        public const string ServiceDisplayName = "سرویس مدیریت بانک";
    }
}
