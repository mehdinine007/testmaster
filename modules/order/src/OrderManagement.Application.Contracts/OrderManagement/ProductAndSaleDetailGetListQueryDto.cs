﻿using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class ProductAndSaleDetailGetListQueryDto
    {
        public string NodePath { get; set; }
        public List<PropertyFilter> PropertyFilters { get; set; }
        public AttachmentEntityTypeEnum? attachmentType { get; set; }
        public bool HasProperty { get; set; }
    }
}
