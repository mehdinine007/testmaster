using System.ComponentModel.DataAnnotations;

namespace UserManagement.Application.Contracts.Models;

public class ChangeMobileDto
{
    public Guid UserId { get; set; }
    public string Mobile { get; set; }
    public string SmsCode { get; set; }
}
