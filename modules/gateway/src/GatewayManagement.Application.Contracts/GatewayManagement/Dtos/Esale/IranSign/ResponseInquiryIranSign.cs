using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale.IranSign
{
    public class ResponseInquiryIranSign
    {
        public string message { get; set; }
        public int resultCode { get; set; }
        public bool Success { get; set; }
        public ResponseBody responseBody { get; set; }

    }
}
