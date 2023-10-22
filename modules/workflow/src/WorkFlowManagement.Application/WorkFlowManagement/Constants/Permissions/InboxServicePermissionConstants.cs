using Esale.Core.Bases;

namespace WorkFlowManagement.Application.Contracts;

public class InboxServicePermissionConstants : BasePermissionConstants
{
    public const string GetInbox = "000300010001";

    public override string ModuleIdentifier => "0003";

    public override string ServiceIdentifier => "0001";

    public override string ServiceDisplayName => "سرویس کارتابل جاری";
}
