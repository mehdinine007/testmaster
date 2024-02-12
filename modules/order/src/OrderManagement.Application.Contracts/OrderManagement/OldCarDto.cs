using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class OldCarDto
    {
        public string Vehicle { get; set; }
        public string Nationalcode { get; set; }
        public string Vin { get; set; }
        public string ChassiNo { get; set; }
        public string EngineNo { get; set; }
    }
}
