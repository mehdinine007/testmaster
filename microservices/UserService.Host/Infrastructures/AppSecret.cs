namespace UserService.Host.Infrastructures;

public class AppSecret
{
    public bool IsEnabled { get; set; }
    public string SecurityKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
}
