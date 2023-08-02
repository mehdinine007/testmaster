﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class ProductProperty : FullAuditedEntity<Guid>
    {
        public int ProductId { get; set; }
        public List<PropertyCategory> PropertyCategories { get; set; }
    }
}
