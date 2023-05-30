using System;

namespace OrderManagement.Application.Contracts
{
    public class CommitOrderResultDto
    {
        public Guid UId { get; set; }
        public bool PaymentGranted { get; set; }
        public PaymentMethodConfiguration PaymentMethodConigurations { get; set; }
    }

    public class PaymentMethodConfiguration
    {
        public string Token { get; set; }
        public int StatusCode { get; set; }
        public string Url { get; set; }
        public string Message { get; set; }
    }
}