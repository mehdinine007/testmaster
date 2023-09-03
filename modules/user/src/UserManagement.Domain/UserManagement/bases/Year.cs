using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Domain.UserManagement.Bases
{
    public class Year : FullAuditedAggregateRoot<int>
    {
        public string Title { get;set; }
        public virtual ICollection<Season_Company_CarTip> SeasonCompanyCarTip { get; set; }

    }
}
