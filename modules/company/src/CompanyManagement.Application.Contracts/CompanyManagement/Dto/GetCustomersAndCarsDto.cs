﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyManagement.Application.Contracts.CompanyManagement
{
    public class GetCustomersAndCarsDto
    {
        public int SaleId { get; set; }
        [Required]
        public int PageNo { get; set; }
    }
}
