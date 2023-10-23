using System.ComponentModel.DataAnnotations;

namespace UserManagement.Application.Contracts.Models;

public class ChangePasswordDto
{
    [Required]
    public string NewPassWord { get; set; }
    [Required]
    public string SMSCode { get; set; }
    public SMSType SMSLocation { get; set; }

}
