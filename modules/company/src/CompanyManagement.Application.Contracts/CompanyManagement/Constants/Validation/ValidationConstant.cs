namespace CompanyManagement.Application.Contracts.CompanyManagement.Constants.Validation;

public class ValidationConstant
{
    public const string VehicleIsRequired = "وسیله نقلیه اجباری است";
    public const string VehicleIsRequiredId = "0001";
    public const string NationalCodeIsRequired = "کد ملی اجباری است";
    public const string NationalCodeIsRequiredId = "0002";
    public const string VinIsRequired = "vin اجباری است";
    public const string VinIsRequiredId = "0003";
    public const string ChassiIsRequired = "شماره شاسی اجباری است";
    public const string ChassiIsRequiredId = "0004";
    public const string EngineIsRequired = "شماره موتور اجباری است";
    public const string EngineIsRequiredId = "0005";
    public const string ItemNotFound = "مورد یافت نشد";
    public const string ItemNotFoundId = "0006";
    public const string Forbiden = "عملیات برای شما مجاز نیست";
    public const string ForbidenId = "0007";
    public const string CompanyIdNotFound = "شرکت مربوط به این سفارش یافت نشد";
    public const string CompanyIdNotFoundId = "0008";
    public const string OrderIsNotRelatedToThisCompany = "این سفارش مربوط به شرکت شما نیست";
    public const string OrderIsNotRelatedToThisCompanyId = "0009";
    public const string CompanyPayedPriceIsNotAllowedToBeNull = "برای این سفارش قبلا پرداخت ثبت شده و نمیتواند پرداخت هایش خالی باشد";
    public const string CompanyPayedPriceIsNotAllowedToBeNullId = "0010";
    public const string UnableToCancelOrderWhichHavePayment = "سفارشی که پرداخت داشته باشد امکان تغییر وضعیت به انصراف را ندارد";
    public const string UnableToCancelOrderWhichHavePaymentId = "0011";
    public const string NationalCodeIsWrong = "کدملی صحیح نیست";
    public const string NationalCodeIsWrongId = "0012";
    public const string FactorDateConflict = "مغایرت در تاریخ فاکتور";
    public const string FactorDateConflictId = "0013";
    public const string InviteDateConflict = "مغایرت در تاریخ معرفی ";
    public const string InviteDateConflictId = "0014";
    public const string DeliveryDateConflict = "مغایرت در تاریخ تحویل";
    public const string DeliveryDateConflictId = "0015";
    public const string UserCompanyIdNotValid = "شناسه سازمان معتبر نمی باشد";
    public const string UserCompanyIdNotValid_Id = "0016";
}
public static class RuleSets
{
    public const string Add = "AddList";
    public const string AddId = "0001";
}
