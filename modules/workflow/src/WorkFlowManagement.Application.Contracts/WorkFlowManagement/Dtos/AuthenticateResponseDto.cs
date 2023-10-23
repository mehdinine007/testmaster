using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Auditing;

namespace WorkFlowManagement.Application.Contracts.WorkFlowManagement.Dtos
{
 

    public class AuthenticateResponseDto
    {
        public AuthenticateResult Data { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
        public int ErrorCode { get; set; }
    }

    public class AuthenticateResult
    {
        public string AccessToken { get; set; }

        public string EncryptedAccessToken { get; set; }

        public int ExpireInSeconds { get; set; }


    }
}
