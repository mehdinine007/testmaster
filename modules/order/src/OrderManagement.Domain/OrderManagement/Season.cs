using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class Season : FullAuditedEntity<int>
    {
        public String Name { get; set; }

        public virtual ICollection<Season_Product_Category> SeasonCompanyCarTip { get; set; }
    }

}
