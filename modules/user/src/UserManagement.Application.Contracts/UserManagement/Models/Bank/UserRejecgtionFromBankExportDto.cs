namespace UserManagement.Application.Contracts.Models;

public class UserRejecgtionFromBankExportDto
{
    public string ShebaNumber { get; set; }

    public string AccountNumber { get; set; }

    public string NationalCode { get; set; }
    public decimal Price { get; set; }
    public DateTime? dateTime { get; set; }


}