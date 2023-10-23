using Newtonsoft.Json;
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
        public string ConcurrencyStamp { get; set; }
        public List<ProductPropertyCategoryDto> PropertyCategories { get; set; }

    }
}
