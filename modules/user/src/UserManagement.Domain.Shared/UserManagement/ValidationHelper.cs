using System.Text.RegularExpressions;

namespace UserManagement.Domain.Shared;

public static class ValidationHelper
{

    #region IsEmail
    public const string EmailRegex = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

    public static bool IsEmail(string value)
    {
        if (value.IsNullOrEmpty())
        {
            return false;
        }

        var regex = new Regex(EmailRegex);
        return regex.IsMatch(value);
    }
    #endregion

    #region IsShaba
    public static bool IsShaba(string shaba)
    {
        if (!shaba.ToLower().StartsWith("ir"))
        {
            shaba = "IR" + shaba;
        }
        if (shaba.IsNullOrWhiteSpace())
        {
            return false;
        }
        shaba = shaba.Replace(" ", "").ToLower();
        //بررسی رشته وارد شده برای اینکه در فرمت شبا باشد
        var isSheba = Regex.IsMatch(shaba, "^[a-zA-Z]{2}\\d{2} ?\\d{4} ?\\d{4} ?\\d{4} ?\\d{4} ?[\\d]{0,2}",
            RegexOptions.Compiled);

        if (!isSheba)
            return false;
        //طول شماره شبا را چک میکند کمتر نباشد
        if (shaba.Length != 26)
            return false;
        shaba = shaba.ToLower();
        //بررسی اعتبار سنجی اصلی شبا
        ////ابتدا گرفتن چهار رقم اول شبا
        var get4FirstDigit = shaba.Substring(0, 4);
        ////جایگزین کردن عدد 18 و 27 به جای آی و آر
        var replacedGet4FirstDigit = get4FirstDigit.ToLower().Replace("i", "18").Replace("r", "27");
        ////حذف چهار رقم اول از رشته شبا
        var removedShebaFirst4Digit = shaba.Replace(get4FirstDigit, "");
        ////کانکت کردن شبای باقیمانده با جایگزین شده چهار رقم اول
        var newSheba = removedShebaFirst4Digit + replacedGet4FirstDigit;
        ////تبدیل کردن شبا به عدد  - دسیمال تا 28 رقم را نگه میدارد
        var finalLongData = Convert.ToDecimal(newSheba);
        ////تقسیم عدد نهایی به مقدار 97 - اگر باقیمانده برابر با عدد یک شود این رشته شبا صحیح خواهد بود
        var finalReminder = finalLongData % 97;
        if (finalReminder == 1)
        {
            return true;
        }
        return false;
    }
    #endregion

    #region IsNationalCode
    public static bool IsNationalCode(string nationalCode)
    {
        try
        {
            if (nationalCode.Length != 10)
            {
                return false;
            }
            //در صورتی که رقم‌های کد ملی وارد شده یکسان باشد
            var allDigitEqual = new[] { "0000000000", "1111111111", "2222222222", "3333333333", "4444444444", "5555555555", "6666666666", "7777777777", "8888888888", "9999999999" };
            if (allDigitEqual.Contains(nationalCode)) return false;


            //عملیات شرح داده شده در بالا
            var chArray = nationalCode.ToCharArray();
            var num0 = Convert.ToInt32(chArray[0].ToString()) * 10;
            var num2 = Convert.ToInt32(chArray[1].ToString()) * 9;
            var num3 = Convert.ToInt32(chArray[2].ToString()) * 8;
            var num4 = Convert.ToInt32(chArray[3].ToString()) * 7;
            var num5 = Convert.ToInt32(chArray[4].ToString()) * 6;
            var num6 = Convert.ToInt32(chArray[5].ToString()) * 5;
            var num7 = Convert.ToInt32(chArray[6].ToString()) * 4;
            var num8 = Convert.ToInt32(chArray[7].ToString()) * 3;
            var num9 = Convert.ToInt32(chArray[8].ToString()) * 2;
            var a = Convert.ToInt32(chArray[9].ToString());

            var b = (((((((num0 + num2) + num3) + num4) + num5) + num6) + num7) + num8) + num9;
            var c = b % 11;

            return (((c < 2) && (a == c)) || ((c >= 2) && ((11 - c) == a)));
        }
        catch { return false; }
    }
    #endregion

    #region IsMobileNumber
    //ToDO(Severe-limitation):kavian
    public static bool IsMobileNumber(string mobileNumber)
    {
        try
        {
            if (mobileNumber.Length != 11)
            {
                return false;
            }

            if (!mobileNumber.All(char.IsDigit))
            {
                return false;
            }

            var allDigitEqual = new[] { "00000000000", "11111111111", "22222222222", "33333333333", "44444444444", "55555555555", "66666666666", "77777777777", "88888888888", "99999999999" };
            if (allDigitEqual.Contains(mobileNumber)) return false;

            var StartsWith = mobileNumber.StartsWith("09");
            if (StartsWith) return StartsWith;

            var chArray = mobileNumber.ToCharArray();
            var num0 = Convert.ToInt32(chArray[0].ToString()) * 10;
            var num2 = Convert.ToInt32(chArray[1].ToString()) * 9;
            var num3 = Convert.ToInt32(chArray[2].ToString()) * 8;
            var num4 = Convert.ToInt32(chArray[3].ToString()) * 7;
            var num5 = Convert.ToInt32(chArray[4].ToString()) * 6;
            var num6 = Convert.ToInt32(chArray[5].ToString()) * 5;
            var num7 = Convert.ToInt32(chArray[6].ToString()) * 4;
            var num8 = Convert.ToInt32(chArray[7].ToString()) * 3;
            var num9 = Convert.ToInt32(chArray[8].ToString()) * 2;
            var num10 = Convert.ToInt32(chArray[9].ToString()) * 1;
            var a = Convert.ToInt32(chArray[10].ToString());

            var b = (((((((num0 + num2) + num3) + num4) + num5) + num6) + num7) + num8) + num9 + num10;
            var c = b % 11;

            return (((c < 2) && (a == c)) || ((c >= 2) && ((11 - c) == a)));
        }
        catch (Exception)
        {

            return false;
        }
    }
    #endregion

}