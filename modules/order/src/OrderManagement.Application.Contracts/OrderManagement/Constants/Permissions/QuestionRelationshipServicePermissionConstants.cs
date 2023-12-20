using Licence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement.Constants.Permissions
{
    public class QuestionRelationshipServicePermissionConstants
    {
        public const string GetById = ConstantInfo.ModuleOrder + ServiceIdentifier + "0001";
        public const string GetById_DisplayName = "نمایش ارتباط سوال پرسشنامه توسط شناسه";

        public const string GetList = ConstantInfo.ModuleOrder + ServiceIdentifier +"0002";
        public const string GetList_DisplayName = "نمایش ارتباط سوالات پرسشنامه توسط سوال";

        public const string Delete = ConstantInfo.ModuleOrder + ServiceIdentifier + "0003";
        public const string Delete_DisplayName = "حذف";

        public const string Add = ConstantInfo.ModuleOrder + ServiceIdentifier + "0004";
        public const string Add_DisplayName = "ثبت";

        public const string Update = ConstantInfo.ModuleOrder + ServiceIdentifier + "0005";
        public const string Update_DisplayName = "بروزرسانی";

        public const string ServiceIdentifier = "0024";
        public const string ServiceDisplayName = "ارتباط سوالات پرسشنامه";
    }
}
