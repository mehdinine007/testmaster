using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFG.Core.Bases
{
    public class MongoConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string DataBase { get; set; }
    }
}
