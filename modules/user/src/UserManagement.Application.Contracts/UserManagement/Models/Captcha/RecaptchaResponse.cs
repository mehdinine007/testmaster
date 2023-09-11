namespace UserManagement.Application.Contracts.Models;

public class RecaptchaResponse
{
    public bool Success { get; set; }
    public string Error { get; set; }
    public string ErrorCode { get; set; }
}
