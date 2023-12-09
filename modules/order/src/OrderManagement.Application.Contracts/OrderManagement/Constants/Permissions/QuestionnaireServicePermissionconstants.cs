using IFG.Core.Bases;
using Licence;

namespace OrderManagement.Application.Contracts;

public class QuestionnaireServicePermissionconstants 
{
    public const string LoadQuestionnaireTree = ConstantInfo.ModuleOrder + ServiceIdentifier + "0001";
    public const string LoadQuestionnaireTree_DisplayName = "بارگذاری درخت پرسشنامه";
    public const string SubmitAnswer = ConstantInfo.ModuleOrder + ServiceIdentifier + "0002";
    public const string SubmitAnswer_DisplayName = "ثبت پاسخ";


    public const string ServiceIdentifier = "0005";
    public const string ServiceDisplayName = "سرویس مدیریت پرسشنامه";
}
