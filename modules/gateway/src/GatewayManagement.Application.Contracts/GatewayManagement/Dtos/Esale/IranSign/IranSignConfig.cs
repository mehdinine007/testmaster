using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale.IranSign
{
    public class IranSignConfig
    {
        public string BaseUrl { get; set; }
        public string ApiKey { get; set; }
        public string OwnerUserName { get; set; }
        public int ProductUid { get; set; }
        public string KeyStoreType { get; set; }
    }
}
