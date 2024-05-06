using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale.IranSign
{
    public class WorkFlowInfo
    {
        public string title { get; set; }
        public string ownerUsername { get; set; }
        public string description { get; set; }
        public string workflowLanguage { get; set; }
        public string workflowPolicyType { get; set; }
        public string workflowType { get; set; }
        public Document document { get; set; }
       
    }
}
