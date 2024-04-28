using GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale.IranSign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale
{
    public class InquirySignOutputDto
    {
        public string Message { get; set; }
        public int ResultCode { get; set; }
        public bool Success { get; set; }
        public string State { get; set; }
        public string DocumentLink { get; set; }
        public string SignedDocumentLink { get; set; }
        
    }
}
