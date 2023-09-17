using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.Shared;

namespace UserManagement.Domain.UserManagement.SendBox.Dtos
{
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
}
