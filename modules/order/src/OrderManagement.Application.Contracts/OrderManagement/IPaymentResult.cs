namespace OrderManagement.Application.Contracts
{
    public interface IPaymentResult
    {
        int Status { get; set; }

        int PaymentId { get; set; }

        string Message { get; set; }

        int OrderId { get; set; }
    }
}