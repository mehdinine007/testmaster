using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Application.Contracts.UserManagement.Models
{
    public class SendSmsLog
    {
        public string Description { get; set; }
        public Dictionary<string, object> Data { get; set; }
    }
}
