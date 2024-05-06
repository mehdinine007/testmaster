using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Company
{
    public class CompanyServicePermissionConstants
    {
        public const string GetCustomersAndCars = ConstantInfo.ModuleCompany + ServiceIdentifier + "0001";
        public const string GetCustomersAndCars_DisplayName = "نمایش مشتری ها و خودروها";
        public const string InsertCompanyProduction = ConstantInfo.ModuleCompany + ServiceIdentifier + "0002";
        public const string InsertCompanyProduction_DisplayName = "ایجاد برنامه تولید خودرو ساز";
        public const string SubmitOrderInformations = ConstantInfo.ModuleCompany + ServiceIdentifier + "0003";
        public const string SubmitOrderInformations_DisplayName = "ثبت اطلاعات سفارش";
        public const string GetRecentCustomerAndOrder = ConstantInfo.ModuleCompany + ServiceIdentifier + "0004";
        public const string GetRecentCustomerAndOrder_DisplayName = "گرفتن اطلاعات مشتریان برای شرکت های خودرو ساز";
        public const string SaveOrderInformation = ConstantInfo.ModuleCompany + ServiceIdentifier + "0005";
        public const string SaveOrderInformation_DisplayName = "ثبت اطلاعات سفارش به صورت تکی";


        public const string ServiceIdentifier = "0001";
        public const string ServiceDisplayName = "سرویس شرکت ها";
    }
}
