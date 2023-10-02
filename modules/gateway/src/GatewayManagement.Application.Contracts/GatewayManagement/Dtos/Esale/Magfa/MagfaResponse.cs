using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale
{
    public class MagfaResponse
    {
        public int status { get; set; }
        public List<MagfaMessage> messages { get; set; }
    }

    public class MagfaMessage
    {
        public int status { get; set; }
        public long id { get; set; }
        public long userId { get; set; }
        public int parts { get; set; }
        public decimal tariff { get; set; }
        public string alphabet { get; set; }
        public string recipient { get; set; }
    }






}
