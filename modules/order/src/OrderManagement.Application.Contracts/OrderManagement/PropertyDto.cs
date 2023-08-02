using OrderManagement.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts
{
    public class PropertyDto
    {
        public Guid Id { get; set; }
        public string Tilte { get; set; }
        public PropertyTypeEnum Type { get; set; }
        public string TypeTitle { get; set; }
        public string Value { get; set; }
        public int Priority { get; set; }
        public List<DropDownItemDto> DropDownItems { get; set; }
    }

    public class DropDownItemDto
    {
        public string Title { get; set; }
        public int Value { get; set; }
    }
}
