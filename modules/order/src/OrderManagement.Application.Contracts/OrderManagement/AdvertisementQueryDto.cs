﻿using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class AdvertisementQueryDto
    {
        public int Id { get; set; }
        public string AttachmentType { get; set; } 
        public string Attachmentlocation { get; set; } 


    }
}
