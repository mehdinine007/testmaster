﻿using System;
using System.Collections.Generic;
using System.Text;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class SaleSchemaDto
    {

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int SaleStatus { get; set; }

    }
}
