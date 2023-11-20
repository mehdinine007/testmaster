using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;

namespace CompanyManagement.Application.Contracts
{
    public class AuthenticateReqDto
    {
       
        public string userID { get; set; }
        public string userPWD { get; set; }
    }
}
