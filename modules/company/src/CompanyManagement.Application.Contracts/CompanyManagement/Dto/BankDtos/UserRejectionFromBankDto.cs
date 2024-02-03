using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyManagement.Application.Contracts.CompanyManagement.Dto.BankDtos
{
    public class UserRejectionFromBankDto
    {
        [MaxLength(10)]
        public string nationalcode { get; set; }
        [Required]
        public string bankName { get; set; }
        [Required]
        public decimal price { get; set; }
        [Required]
        public DateTime dateTime { get; set; }
        [Required]
        public string accountNumber { get; set; }
        [MaxLength(26)]
        [Required]
        public string shabaNumber { get; set; }
        public long UserId { get; set; }
        public string CarMaker { get; set; }
    }
}
