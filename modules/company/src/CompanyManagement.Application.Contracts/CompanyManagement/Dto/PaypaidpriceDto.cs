﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyManagement.Application.Contracts.CompanyManagement
{
    public class PaypaidPriceDto
    {
        public DateTime TranDate { get; set; }
        public long PayedPrice { get; set; }
    }
}
