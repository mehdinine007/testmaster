using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Domain.UserManagement.SendBox.Dtos
{
    public class RegistrationSMSDto
    {
        public string SMSCode { get; set; }
        public DateTime LastSMSSend { get; set; }
    }
}
