using System.ComponentModel.DataAnnotations;

namespace UserManagement.Application.Contracts.Models;

public class AdvocacyUsersFromBankWithCompanyDto
{

    [MaxLength(10)]
    public string nationalcode { get; set; }
    [Required]
    public string bankName { get; set; }
    [Required]
    public decimal price { get; set; }
    [Required]
    public string accountNumber { get; set; }
    [Required]
    [MaxLength(26)]
    public string shabaNumber { get; set; }
}