using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class SiteStructureAddOrUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public SiteStructureTypeEnum Type { get; set; }
        public string Description { get; set; }

    }
}
