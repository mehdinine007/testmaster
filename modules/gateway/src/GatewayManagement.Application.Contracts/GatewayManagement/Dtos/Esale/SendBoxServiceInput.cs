using GatewayManagement.Application.Contracts.GatewayManagement.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayManagement.Application.Contracts.GatewayManagement.Dtos
{
    public class SendBoxServiceInput
    {
        public string Recipient { get; set; }
        public string Text { get; set;}
        public ProviderSmsTypeEnum  Provider { get; set; }
        public TypeMessageEnum Type { get; set; }
    }
}
