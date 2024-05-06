using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale.IranSign
{
    public class DataFields
    {
        public string dataFieldType { get; set; }
        public int pageNumber { get; set; }
        public string tag { get; set; }
        public decimal topRel { get; set; }
        public decimal leftRel { get; set; }
        public decimal heightRel { get; set; }
        public decimal widthRel { get; set; }
    }
}
