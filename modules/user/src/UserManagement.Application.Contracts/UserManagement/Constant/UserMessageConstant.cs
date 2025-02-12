﻿namespace UserManagement.Application.Contracts;

public class UserMessageConstant
{

    public static string CreateUserDefultRoleCodeNotFound = "تنظیمات دسترسی تعریف نشده است";
    public static string CreateUserDefultRoleCodeNotFoundId = "1010";

    public static string UserDataAccessNationalCodeNotFound = "کاربر گرامی شما دسترسی به انجام این عملیات را ندارید!";
    public static string UserDataAccessNationalCodeNotFoundId = "";

    public static string UpdatePhoneNumberInvalidFormat = "شماره تلفن معتبر نیست";
    public static string UpdatePhoneNumberInvalidFormatId = "1010101";

    public static string UpdatePhoneNumberUserIdIsWrong = "کاربر یافت نشد";
    public static string UpdatePhoneNumberUserIdIsWrongId = "1010100";

    public static string UpdatePhoneNumberWrongSmsCode = "کد پیامک ارسالی صحیح نمی باشد";
    public static string UpdatePhoneNumberWrongSmsCodeId = "10101010101";
    public const string SendSmsErorr = "ارسال پیامک با خطا مواجه شد";
    public const string SendSmsErorrId = "9009";
    public const string CaptchaErorr = "خطای کپچا";
    public const string CaptchaErorrId = "9010";
    public const string UsernameIsNotCorrect = "نام کاربری صحیح نمی باشد";
    public const string UsernameIsNotCorrectId = "9011";
    public const string NationalCodeOrMobileNotCorrect = "کد ملی یا شماره موبایل صحیح نمی باشد";
    public const string NationalCodeOrMobileNotCorrectId = "9012";
    public const string TextSent2MinsAgo = "در دو دقیقه گذشته برای شما پیامک ارسال شده است";
    public const string TextSent2MinsAgoId = "9013";

 


    public static string CaptchaNotValid = "کپچا وارد شده صحیح نمی باشد";
    public static string CaptchaNotValidId = "1002";
    public static string ShabaNotValid = "شماره شبا وارد شده نامعتبر می باشد";
    public static string ShabaNotValidId = "1003";
    public static string NationalCodeNotValid = "کد ملی وارد شده نامعتبر می باشد";
    public static string NationalCodeNotValidId = "1004";
    public static string NationalCodeExists = "کد ملی وارد شده قبلا در سامانه ثبت شده";
    public static string NationalCodeExistsId = "1005";
    public static string UserInsertError = "خطا در ثبت کاربر";
    public static string UserInsertErrorId = "1006";
    public static string IsMobileNumberMessage = "شماره موبایل صحیح نمی باشد";

}
