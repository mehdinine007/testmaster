using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale.IranSign
{
    public class SignedDocumentInfo
    {
        public string fileName { get; set; }
        public string fileSize { get; set; }
        public string fileType { get; set; }
        public string signedDocumentReference { get; set; }
        public string signedDocumentLink { get; set; }
    }
}
