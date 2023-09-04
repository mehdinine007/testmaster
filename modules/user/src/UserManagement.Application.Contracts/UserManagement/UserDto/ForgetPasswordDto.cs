
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Domain.UserManagement
{
    public class ForgetPasswordDto
    {
        [Required]
        [MaxLength(10)]
        public string NationalCode { get; set; }
        [Required]
        [MaxLength(11)]
        public string Mobile { get; set; }
    }
}
