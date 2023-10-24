using Esale.Core.Bases;

namespace WorkFlowManagement.Application.Contracts;

public class InboxServicePermissionConstants : BasePermissionConstants
{
    public const string GetInbox = "000400010001";
    public const string GetInbox_DisplayName = "نمایش کارتابل جاری";

    public override string ModuleIdentifier => "0004";

    public override string ServiceIdentifier => "0001";

    public override string ServiceDisplayName => "سرویس کارتابل جاری";
}
