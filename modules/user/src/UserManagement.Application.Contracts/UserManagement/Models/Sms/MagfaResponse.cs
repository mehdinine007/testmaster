namespace UserManagement.Application.Contracts.Models;

public class MagfaResponse
{
    public int status { get; set; }
    public List<MagfaMessage> messages { get; set; }
}
