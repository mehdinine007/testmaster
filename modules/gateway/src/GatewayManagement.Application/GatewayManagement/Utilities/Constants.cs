namespace GatewayManagement.Application.Utilities
{
    public static class Constants
    {
        #region Url
        public const string IranKishGetTokenUrl = "https://ikc.shaparak.ir/api/v3/tokenization/make";
        public const string IranKishVerifyUrl = "https://ikc.shaparak.ir/api/v3/confirmation/purchase";
        public const string IranKishInquiryUrl = "https://ikc.shaparak.ir/api/v3/inquiry/single";
        public const string IranKishReverseUrl = "https://ikc.shaparak.ir/api/v3/confirmation/reversePurchase";
        #endregion
    }
}
