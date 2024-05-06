using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale.IranSign
{
    public class SignatureImageTextParameter
    {
        public string customText { get; set; }
        public bool name { get; set; }
        public bool signDate { get; set; }
    }
}
