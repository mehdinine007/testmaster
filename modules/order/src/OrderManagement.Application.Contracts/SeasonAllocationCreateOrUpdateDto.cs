using OrderManagement.Domain.Shared.OrderManagement.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts
{
    public class SeasonAllocationCreateOrUpdateDto
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Title { get; set; }
        public SeasonTypeEnum SeasonId { get; set; }
        public int Year { get; set; }
    }
}
