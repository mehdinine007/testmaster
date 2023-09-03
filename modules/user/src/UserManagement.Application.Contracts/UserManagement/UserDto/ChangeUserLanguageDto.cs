using System.ComponentModel.DataAnnotations;

namespace UserManagement.Domain.UserManagement.UserDto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}