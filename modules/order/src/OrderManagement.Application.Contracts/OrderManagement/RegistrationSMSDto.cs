using System;

namespace OrderManagement.Application.Contracts
{
    public class SMSPayloadDto
    {
        public string SMSCode { get; set; }
        public DateTime LastSMSSend { get; set; }
    }

    public class SMSDto
    {
        public string SMSCode { get; set; }
        public DateTime LastSMSSend { get; set; }
    }

}