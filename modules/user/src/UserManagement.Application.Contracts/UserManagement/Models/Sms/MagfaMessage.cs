namespace UserManagement.Application.Contracts.Models;

public class MagfaMessage
{
    public int status { get; set; }
    public long id { get; set; }
    public long userId { get; set; }
    public int parts { get; set; }
    public decimal tariff { get; set; }
    public string alphabet { get; set; }
    public string recipient { get; set; }
}