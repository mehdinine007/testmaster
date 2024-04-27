using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale.IranSign
{
    public class ResponseBody
    {
        public string workflowTicket { get; set; }
        public string dataType { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string createDate { get; set; }
        public DateTime updateDate { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string workflowType { get; set; }
        public string state { get; set; }
        public string ownerId { get; set; }
        public DocumentInfo documentInfo { get; set; }
        //public List<Recipients> recipients { get; set; }
        //public SignedDocumentInfo signedDocumentInfo { get; set; }
    }
}
