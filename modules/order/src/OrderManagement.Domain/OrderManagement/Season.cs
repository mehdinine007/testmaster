﻿using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class Season : FullAuditedAggregateRoot<int>
    {
        public String Name { get; set; }

        public virtual ICollection<Season_Company_CarTip> SeasonCompanyCarTip { get; set; }
    }

}
