using Licence;

namespace OrderManagement.Application.Contracts.Constants.Permissions
{
    public class SeasonCompanyProductServicePermissionConstants
    {
        public const string Create = ConstantInfo.ModuleOrder + ServiceIdentifier + "0001";
        public const string Create_DisplayName = "ایجاد برنامه تولید خودرو ساز به اضای فصل";
        public const string Delete = ConstantInfo.ModuleOrder + ServiceIdentifier + "0002";
        public const string Delete_DisplayName = "حذف برنامه تولید خودروساز";
        public const string GetById = ConstantInfo.ModuleOrder + ServiceIdentifier + "0003";
        public const string GetById_DisplayName = "گرفتن برنامه تولید خئروساز با شناسه";
        public const string Update = ConstantInfo.ModuleOrder + ServiceIdentifier + "0004";
        public const string Update_DisplayName = "بروزرسانی برنامه تولید خودروساز";
        public const string ServiceIdentifier = "0027";
        public const string ServiceDisplayName = "سرویس مدیریت میزان تولید خودروسازان در فصول مختلف";
    }
}
