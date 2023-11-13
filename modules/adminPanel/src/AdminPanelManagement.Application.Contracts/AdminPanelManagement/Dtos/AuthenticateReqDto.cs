using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanelManagement.Application.Contracts.AdminPanelManagement.Dtos
{
    public class AuthenticateReqDto
    {
        public string UserNameOrEmailAddress { get; set; }
        public string Password { get; set; }
    }
}
