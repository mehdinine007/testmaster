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
        public List<AdvancedSearchDto> AdvancedSearch { get; set; }
        public List<AttachmentEntityTypeEnum>? attachmentType { get; set; } = null;
        public bool HasProperty { get; set; }
        public int? ESaleTypeId { get; set; }
    }
}
