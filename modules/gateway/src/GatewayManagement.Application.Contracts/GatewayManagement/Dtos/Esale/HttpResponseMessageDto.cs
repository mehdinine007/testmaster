using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayManagement.Application.Contracts.GatewayManagement.Dtos
{
    public class HttpResponseMessageDto
    {
        public bool IsSuccessStatusCode { get; set; }
        public string StringContent { get; set; }
    }
}
