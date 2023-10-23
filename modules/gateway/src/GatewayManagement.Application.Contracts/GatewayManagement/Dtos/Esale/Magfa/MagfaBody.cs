using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale
{
    public class MagfaBody
    {
        public List<string> senders { get; set; }
        public List<string> messages { get; set; }
        public List<string> recipients { get; set; }
    }
}
