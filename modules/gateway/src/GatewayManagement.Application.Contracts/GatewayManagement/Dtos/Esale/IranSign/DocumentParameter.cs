using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale.IranSign
{
    public class DocumentParameter
    {
        public List<DataFields> dataFields { get; set; }
        public SignatureImageTextParameter signatureImageTextParameter { get; set; }
    }
}
