using Esale.Core.Bases;

namespace OrderManagement.Application.Contracts;

public class QuestionnaireServicePermissionconstants : BasePermissionConstants
{
    public const string LoadQuestionnaireTree = "000100050001";
    public const string SubmitAnswer = "000100050002";

    public override string ModuleIdentifier => "0001";

    public override string ServiceIdentifier => "0005";
}
