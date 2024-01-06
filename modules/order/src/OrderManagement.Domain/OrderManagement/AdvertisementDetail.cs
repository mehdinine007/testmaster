using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain.OrderManagement
{
    public class AdvertisementDetail: FullAuditedEntity<int>
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public  Advertisement Advertisement { get; set; }
        public  int AdvertisementId { get; set; }
        public int Priority { get; set; }
    }
}
