﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts
{
    public class SiteStructureDto
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Title { get; set; }
        public SiteStructureTypeEnum Type { get; set; }
        public string TypeTitle { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public List<AttachmentViewModel> Attachments { get; set; }
        public IEnumerable<dynamic> CarouselData { get; set; }
    }
}
