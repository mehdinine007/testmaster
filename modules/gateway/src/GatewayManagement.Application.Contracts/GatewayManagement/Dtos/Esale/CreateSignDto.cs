using GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale.IranSign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayManagement.Application.Contracts.Dtos.Esale
{
    public class CreateSignDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string DocumentName { get; set; }
        public string DocumentData { get; set; }
        public string RecipientUsername { get; set; }
        public string DocumentParameter { get; set; }
    }
}
