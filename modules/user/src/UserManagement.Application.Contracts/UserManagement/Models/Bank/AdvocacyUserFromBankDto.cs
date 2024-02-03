namespace UserManagement.Application.Contracts.Models;

public class AdvocacyUserFromBankDto
{
    public string ShebaNumber { get; set; }
    public string AccountNumber { get; set; }
    public int? BankId { get; set; }
    public int GenderCode { get; set; }
}
