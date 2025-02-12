﻿namespace OrderManagement.Application.Contracts
{
    public class PaymentHandShakeViewModel
    {
        //0 means handShake Success
        public int StatusCode { get; set; }
        public string Message { get; set; }        
        public int PaymentId { get; set; }
        public string Token { get; set; }
        public string HtmlContent { get; set; }
        public string PspJsonResult { get; set; }
    }
}