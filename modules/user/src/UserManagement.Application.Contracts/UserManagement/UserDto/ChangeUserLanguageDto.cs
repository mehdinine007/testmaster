using System.ComponentModel.DataAnnotations;

namespace UserManagement.Domain.UserManagement
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}