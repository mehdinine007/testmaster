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
    [Required]
    public string Recipient { get; set; }
    [Required]
    public string NationalCode { get; set; }
    [Required]
    public SMSType SMSLocation { get; set; }
}
