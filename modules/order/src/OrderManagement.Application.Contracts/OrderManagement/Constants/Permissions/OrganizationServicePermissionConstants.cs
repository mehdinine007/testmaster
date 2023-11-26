using IFG.Core.Bases;

namespace OrderManagement.Application.Contracts;

public class OrganizationServicePermissionConstants : BasePermissionConstants
{

    public const string GetById = "000200220001";
    public const string GetById_DisplayName = "نمایش سازمان با شناسه";
    public const string GetAll = "000200220002";
    public const string GetAll_DisplayName = "نمایش همه سازمان ها";
    public const string Delete = "000200220003";
    public const string Delet_DisplayName = "حذف";
    public const string Save = "000200220004";
    public const string Save_DisplayName = "ثبت";
    public const string Update = "000200220005";
    public const string Update_DisplayName = "بروزرسانی";
    public const string UploadFile = "000200220006";
    public const string UploadFile_DisplayName = "آپلود فایل";


    public override string ModuleIdentifier => "0002";

    public override string ServiceIdentifier => "0022";

    public override string ServiceDisplayName => "سرویس مدیریت سازمان";
}
