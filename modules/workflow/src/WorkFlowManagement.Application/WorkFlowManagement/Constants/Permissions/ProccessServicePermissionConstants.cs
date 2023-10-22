using Esale.Core.Bases;

namespace WorkFlowManagement.Application.Contracts;

public class ProccessServicePermissionConstants : BasePermissionConstants
{
    public const string StartProcess = "000300020001";
    public const string GetOutBox = "000300020002";
    public const string GetOutBox_Title = "000300020002";

    public override string ModuleIdentifier => "0003";
    public override string ServiceIdentifier => "0002";
    public override string ServiceDisplayName => "سرویس کارتابل ارسال شده";
}
