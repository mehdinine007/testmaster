﻿namespace OrderManagement.Application.Contracts
{
    public class PaymentResultViewModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public int PaymentId { get; set; }
        public string PspJsonResult { get; set; }
    }
}