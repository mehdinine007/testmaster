using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale.IranSign
{
    public class RecipientDocumentParameter
    {
        public List<DataFieldsRespons> dataFields { get; set; }
        public string primaryDocumentReference { get; set; }
        public string primaryDocumentLink { get; set; }
        public string signedDocumentReference { get; set; }
        public string signedDocumentLink { get; set; }
    }
}
