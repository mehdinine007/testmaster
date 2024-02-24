using OrderManagement.Domain.Shared.OrderManagement.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class MoveDto
    {
        public int Id { get; set; }
        public MoveTypeEnum MoveType { get; set; }
    }
}
