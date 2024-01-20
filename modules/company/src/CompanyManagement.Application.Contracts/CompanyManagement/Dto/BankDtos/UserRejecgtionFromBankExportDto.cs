using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyManagement.Application.Contracts.CompanyManagement.Dto.BankDtos
{
    public class UserRejecgtionFromBankExportDto
    {
        public string ShebaNumber { get; set; }

        public string AccountNumber { get; set; }

        public string NationalCode { get; set; }
        public decimal Price { get; set; }
        public DateTime? dateTime { get; set; }
    }
}
