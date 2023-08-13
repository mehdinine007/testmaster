using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain.OrderManagement
{
    public class CarClass: FullAuditedEntity<int>
    {
        public string  Title { get; set; }
        public virtual ICollection<ProductAndCategory> ProductAndCategories { get; set; }
    }
}
