using OrderManagement.Domain.Shared.OrderManagement.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class AdvertisementDetailWithIdDto
    {
        public int Id { get; set; }
        public MoveTypeEnum MoveType { get; set; }
    }
}
