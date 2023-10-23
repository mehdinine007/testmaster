﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatewayManagement.Application.Contracts.GatewayManagement.Dtos.Esale
{
    public class MagfaSendSmsInput
    {
        public string Text { get; set; }
        public string Mobile { get; set; }
        //public MagfaConfig Config { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }
        public string SenderNumber { get; set; }
        public string BaseUrl { get; set; }
    }


}
