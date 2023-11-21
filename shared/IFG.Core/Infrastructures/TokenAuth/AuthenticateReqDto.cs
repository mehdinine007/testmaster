namespace IFG.Core.Infrastructures.TokenAuth;

public class AuthenticateReqDto
{    
    public string UserNameOrEmailAddress { get; set; }
    public string Password { get; set; }
}
