using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts
{
    public class PropertyFilter
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public OperatorFilterEnum Operator { get; set; }
    }
}
