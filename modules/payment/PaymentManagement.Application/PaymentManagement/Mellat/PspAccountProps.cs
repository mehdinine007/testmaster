﻿namespace PaymentManagement.Application.Mellat
{
    public class PspAccountProps
    {
        public int Switch { get; set; }
        public long TerminalId { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string ReportServiceUserName { get; set; }
        public string ReportServicePassword { get; set; }
    }
}
