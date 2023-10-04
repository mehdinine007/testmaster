namespace UserManagement.Application.Contracts.Models.SendBox;

public class HttpResponseMessageDto
{
    public bool Success { get; set; }
    public string Error { get; set; }
    public string ErrorCode { get; set; }
}