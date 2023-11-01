namespace CompanyManagement.Application.Contracts;

public class AuthenticateReqDto
{

    public string UserNameOrEmailAddress { get; set; }
    public string Password { get; set; }
}
