namespace IFG.Core.Infrastructures.TokenAuth;

public class AuthenticateResponseDto
{
    public AuthenticateResultModel Data { get; set; }
    public string Message { get; set; }
    public bool Success { get; set; }
    public int ErrorCode { get; set; }
}
