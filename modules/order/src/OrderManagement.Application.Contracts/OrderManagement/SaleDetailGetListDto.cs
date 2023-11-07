using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class SaleDetailGetListDto
    {

        public string CategoryNode { get; set; }
        public int EsaleTypeId { get; set; }
        public string? AttachmentType { get; set; }
        public string? AttachmentLocation { get; set; }
    }
}
