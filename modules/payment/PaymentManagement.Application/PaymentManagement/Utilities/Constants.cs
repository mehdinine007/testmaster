namespace PaymentManagement.Application.Utilities
{
    public static class Constants
    {
        public const string UnknownError = "خطای نامشخص";
        public const string ErrorInConnectToPsp = "خطا در برقراری ارتباط با درگاه";
        public const string ErrorInHandShakeInput = "خطا در اطلاعات ورودی متد هند شیک";
        public const string ErrorInHandShakeResult = "خطای هند شیک با درگاه";
        public const string HandShakeSuccess = "هند شیک با موفقیت انجام شد";
        public const string ErrorInBackFromPsp = "خطا در برگشت از درگاه";
        public const string ErrorInBackFromPspUrl = "خطا در آدرس درگاه در زمان برگشت از درگاه";
        public const string PaymentSuccessDontSendVerify = "وضعیت پرداخت موفق می باشد، لطفا درخواست تایید پرداخت ارسال نشود";
        public const string PaymentFailedDontSendVerify = "وضعیت پرداخت ناموفق می باشد، لطفا درخواست تایید پرداخت ارسال نشود";
        public const string ErrorInBackFromPspResponseCode = "ارسال کد خطا توسط درگاه";
        public const string BackFromPspSuccess = "برگشت از درگاه با موفقیت انجام شد، لطفا در صورت تمایل درخواست تایید پرداخت را ارسال کنید";
        public const string VerifySuccess = "پرداخت با موفقیت انجام شد";
        public const string VerifyFailed = "پرداخت ناموفق می باشد";
        public const string ErrorInVerify = "خطا در ارسال درخواست تایید پرداخت";
        public const string InquirySuccess = "استعلام پرداخت با موفقیت انجام شد";
        public const string InquiryFailed = "استعلام پرداخت ناموفق می باشد";
        public const string ErrorInInquiry = "خطا در ارسال درخواست استعلام پرداخت";

        #region PaymentLog
        public const string HandShakeStart = "HandShakeStart";
        public const string HandShakeResult = "HandShakeResult";
        public const string HandShakeException = "HandShakeException";
        public const string RedirectToPsp = "RedirectToPsp";
        public const string BackFromPsp = "BackFromPsp";
        public const string DuplicateBackFromPsp = "DuplicateBackFromPsp";
        public const string BackFromPspWithInCorrectUrl = "BackFromPspWithInCorrectUrl";
        public const string BackFromPspException = "BackFromPspException";
        public const string VerifyStart = "VerifyStart";
        public const string VerifyException = "VerifyException";
        public const string VerifyResult = "VerifyResult";
        public const string RetryForVerifyStart = "RetryForVerifyStart";
        public const string RetryForVerifyException = "RetryForVerifyException";
        public const string RetryForVerifyResult = "RetryForVerifyResult";
        public const string InquiryStart = "InquiryStart";
        public const string InquiryException = "InquiryException";
        public const string InquiryResult = "InquiryResult";
        #endregion
        #region PspUrl
        public const string IranKishHandShakeUrl = "https://ikc.shaparak.ir/api/v3/tokenization/make";
        public const string IranKishRedirectUrl = "https://ikc.shaparak.ir/iuiv3/IPG/Index?tokenIdentity=";
        #endregion
    }
}
