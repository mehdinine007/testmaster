namespace UserManagement.Application.Contracts.Models.SendBox;

public class SendBoxServiceDto
{
    public bool Success { get; set; }
    public string DataResult { get; set; }
    public string Message { get; set; }
    public int MessageCode { get; set; }
}
