
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Domain.UserManagement.UserDto
{
    public class ChangePasswordDto
    {
        [Required]
        [MaxLength(10)]
        public string NationalCode { get; set; }
        [Required]
        [MaxLength(11)]
        public string Mobile { get; set; }
        [Required]
        public string PassWord { get; set; }
        [Required]
        public string SMSCode { get; set; }

    }
}
