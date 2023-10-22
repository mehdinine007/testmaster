using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Application.Contracts.Models;

public class AuthenticateResultDto
{
    public AuthenticateResultDto()
    {
        this.Data = new AuthenticateResultModel();
    }
    public AuthenticateResultModel Data { get; set; }
    public string Message { get; set; }
    public bool Success { get; set; }
    public int ErrorCode { get; set; }
}

public class AuthenticateResultModel
{
    public string AccessToken { get; set; }

    public string EncryptedAccessToken { get; set; }

    public int ExpireInSeconds { get; set; }


}
