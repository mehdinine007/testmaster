using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Contracts
{
    public class BankCreateOrUpdateDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PhoneNumber { get; set; }
        public string Url { get; set; }
        public int Priority { get; set; }
    }
}
