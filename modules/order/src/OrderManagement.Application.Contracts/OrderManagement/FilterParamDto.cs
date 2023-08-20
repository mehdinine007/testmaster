using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts
{
    public class FilterParamDto
    {
        public string Key { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public PropertyTypeEnum Type { get; set; }
        public int Priority { get; set; }
        public bool HasProperty { get; set; }
        public List<DropDownItemDto> DropDownItems { get; set; }

    }
}
