using System;

namespace OrderManagement.Application.Contracts
{
    public class PaymentInformationResponseDto
    {
        public int PaymentId { get; set; }

        public string TransactionCode {get;set;}

        public DateTime TransactionDate {get;set;}

        public string TransactionPersianDate {get;set;}
        public int PaymentStatusId { get; set; }
        public long Amount { get; set; }
    }
}