using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts
{
    public class ProductPropertyDto
    {
        public int ProductId { get; set; }
        public List<PropertyCategoryDto> PropertyCategories { get; set; }

    }
}
