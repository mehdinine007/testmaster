namespace PaymentManagement.Application.IranKish
{
    public class JsonResult
    {
        public string responseCode { get; set; }
        public object description { get; set; }
        public bool status { get; set; }
        public Result result { get; set; }
    }
    public class Result
    {
        public string token { get; set; }
        public int initiateTimeStamp { get; set; }
        public int expiryTimeStamp { get; set; }
        public string transactionType { get; set; }
        public BillInfo billInfo { get; set; }
    }
    public class BillInfo
    {
        public string BillId { get; set; }
        public string billPaymentId { get; set; }
    }
}
