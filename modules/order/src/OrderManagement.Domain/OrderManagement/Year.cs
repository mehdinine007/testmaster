using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class Year : FullAuditedEntity<int>
    {
        public string Title { get; set; }
        public virtual ICollection<Season_Company_CarTip> SeasonCompanyCarTip { get; set; }

    }

}
