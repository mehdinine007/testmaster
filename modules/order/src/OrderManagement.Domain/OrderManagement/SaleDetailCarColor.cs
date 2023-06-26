﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain.OrderManagement
{
    public class SaleDetailCarColor:FullAuditedEntity<int>
    {
        public int SaleDetailId { get; set; }
        public virtual SaleDetail SaleDetail { get; set; }

        public int ColorId { get; set; }
        public virtual Color Color { get; set; }
    }
}
