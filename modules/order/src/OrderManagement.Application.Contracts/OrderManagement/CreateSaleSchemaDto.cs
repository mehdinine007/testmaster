using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts.OrderManagement
{
    public class CreateSaleSchemaDto
    {

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int Code { get; set; }
    }
}
