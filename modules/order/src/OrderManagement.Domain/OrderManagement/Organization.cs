using OrderManagement.Domain.OrderManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain.OrderManagement
{
    public class Organization : FullAuditedEntity<int>
    {
        
        public int Code { get; set; }  
        public string Title { get; set; }
        public string Url { get; set; }
        public string EncryptKey { get; set; }

    }
}
