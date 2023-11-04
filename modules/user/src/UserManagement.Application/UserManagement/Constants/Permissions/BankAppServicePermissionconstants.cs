using IFG.Core.Bases;

namespace UserManagement.Application.Constants;

public class BankAppServicePermissionconstants : BasePermissionConstants
{
    public const string GetAdvocacyUserByCompanyId = "000100020001";

    public const string GetAdvocacyUserByCompanyId_DisplayName = "گرفتن کاربران انصرافی برای شرکت ها";

    public const string GetUserRejecttionAdvocacyList = "000100020002";

    public const string GetUserRejecttionAdvocacyList_DisplayName = "لیست افراد انصراف داده از طرح";

    public const string InquiryAdvocacyUserReport = "000100020003";

    public const string InquiryAdvocacyUserReport_DisplayName = "گزارش حساب های وکالتی";


    public const string InquiryUserRejectionFromBank = "000100020004";

    public const string InquiryUserRejectionFromBank_DisplayName = "لیست افراد انصراف داده از طریق بانک";

    public const string SaveAdvocacyUsersFromBank = "000100020005";

    public const string SaveAdvocacyUsersFromBank_DisplayName = "ثبت حساب وکالتی از طرف بانک";

    public const string SaveUserRejectionFromBank = "000100020006";

    public const string SaveUserRejectionFromBank_DisplayName = "ثبت انصراف از طریق بانک";

    public override string ModuleIdentifier => "0001";

    public override string ServiceIdentifier => "0002";

    public override string ServiceDisplayName => "سرویس مدیریت بانک";
}