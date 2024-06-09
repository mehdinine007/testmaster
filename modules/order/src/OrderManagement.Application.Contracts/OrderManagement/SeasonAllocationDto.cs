using OrderManagement.Domain.Shared.OrderManagement.Enums;
using OrderManagement.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class SeasonAllocationDto
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string Title { get; set; }
        public SeasonTypeEnum SeasonId { get; set; }
        public string SeasonTitle { get; set; }
        public int Year { get; set; }
        
    }
}
