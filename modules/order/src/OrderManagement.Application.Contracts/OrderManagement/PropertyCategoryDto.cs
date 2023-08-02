using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts
{
    public class PropertyCategoryDto
    {
        public string Title { get; set; }
        public List<PropertyDto> Properties { get; set; }
        public int Priority { get; set; }
    }
}
