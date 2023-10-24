using Esale.Core.Bases;

namespace OrderManagement.Application.Contracts;

public class QuestionnaireServicePermissionconstants : BasePermissionConstants
{
    public const string LoadQuestionnaireTree = "000200050001";
    public const string LoadQuestionnaireTree_DisplayName = "بارگذاری درخت پرسشنامه";
    public const string SubmitAnswer = "000200050002";
    public const string SubmitAnswer_DisplayName = "ثبت پاسخ";

    public override string ModuleIdentifier => "0002";

    public override string ServiceIdentifier => "0005";

    public override string ServiceDisplayName => "سرویس مدیریت پرسشنامه";
}
