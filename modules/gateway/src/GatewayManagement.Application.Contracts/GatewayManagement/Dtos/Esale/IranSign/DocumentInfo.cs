using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale.IranSign
{
    public class DocumentInfo
    {
        public string fileName { get; set; }
        public string fileSize { get; set; }
        public string fileType { get; set; }
        public string documentReference { get; set; }
        public string documentLink { get; set; }
        public string message { get; set; }
    }
}
