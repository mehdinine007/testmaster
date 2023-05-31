namespace OrderManagement.Application.Contracts
{
    public class IPgCallBackRequest : PspInteractionResult
    {
        public string TransactionCode { get; set; }
    }
}