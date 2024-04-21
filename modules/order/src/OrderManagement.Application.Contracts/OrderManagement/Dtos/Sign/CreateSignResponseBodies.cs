using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement.Dtos.Sign
{
    public class CreateSignResponseBodies
    {
        public string workflowTicket { get; set; }
        public DateTime updateDate { get; set; }
        public DateTime createDate { get; set; }
        public string title { get; set; }
        public string workflowType { get; set; }

    }
}
