﻿using System;
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
       
        public string UserNameOrEmailAddress { get; set; }
        public string Password { get; set; }
    }
}
