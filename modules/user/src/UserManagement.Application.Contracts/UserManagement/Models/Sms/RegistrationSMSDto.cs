namespace UserManagement.Application.Contracts.Models;

public class RegistrationSMSDto
{
    public string SMSCode { get; set; }
    public DateTime LastSMSSend { get; set; }
}
