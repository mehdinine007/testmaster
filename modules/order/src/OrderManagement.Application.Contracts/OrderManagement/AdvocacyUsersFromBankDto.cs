using System;
using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Application.Contracts
{
    public class AdvocacyUsersFromBankDto
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
        [Required]
        [MaxLength(26)]
        public string shabaNumber { get; set; }
        public long UserId { get; set; }
        public string CarMaker { get; set; }
    }
}