using System.ComponentModel.DataAnnotations;

namespace UserManagement.Application.Contracts.Models;

public class ForgetPasswordDto
{
    [Required]
    [MaxLength(10)]
    public string NationalCode { get; set; }
    [Required]
    [MaxLength(11)]
    public string Mobile { get; set; }
}
