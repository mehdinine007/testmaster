using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Application.Contracts.Models;

public class AuthenticateResultModel
{
    public AuthenticateResultModel()
    {
        this.Data = new AuthenticateResult();
    }
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
