using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Permission.Order
{
    public class QuestionnaireServicePermissionconstants
    {
        public const string LoadQuestionnaireTree = ConstantInfo.ModuleOrder + ServiceIdentifier + "0001";
        public const string LoadQuestionnaireTree_DisplayName = "بارگذاری درخت پرسشنامه";
        public const string SubmitAnswer = ConstantInfo.ModuleOrder + ServiceIdentifier + "0002";
        public const string SubmitAnswer_DisplayName = "ثبت پاسخ";


        public const string ServiceIdentifier = "0005";
        public const string ServiceDisplayName = "سرویس مدیریت پرسشنامه";
    }
}
