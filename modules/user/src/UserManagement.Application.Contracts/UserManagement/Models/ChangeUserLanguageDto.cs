using System.ComponentModel.DataAnnotations;

namespace UserManagement.Application.Contracts.Models;

public class ChangeUserLanguageDto
{
    [Required]
    public string LanguageName { get; set; }
}