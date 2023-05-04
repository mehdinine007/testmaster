using System;

namespace OrderManagement.Application.Contracts
{
    public class RegistrationSMSDto
    {
        public string SMSCode { get; set; }
        public DateTime LastSMSSend { get; set; }
    }
}