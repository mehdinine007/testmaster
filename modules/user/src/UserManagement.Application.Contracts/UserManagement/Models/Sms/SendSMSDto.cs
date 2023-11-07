using System.ComponentModel.DataAnnotations;

namespace UserManagement.Application.Contracts.Models;

public class SendSMSDto
{
    [Required]
    public string CT { get; set; }
    [Required]
    public string CIT { get; set; }
    [Required]
    public string CK { get; set; }
    public string Recipient { get; set; }
    public string NationalCode { get; set; }
    [Required]
    public SMSType SMSLocation { get; set; }
}
