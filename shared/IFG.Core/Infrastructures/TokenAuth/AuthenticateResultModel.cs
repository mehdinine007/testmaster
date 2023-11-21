namespace IFG.Core.Infrastructures.TokenAuth;

public class AuthenticateResultModel
{
    public string AccessToken { get; set; }

    public string EncryptedAccessToken { get; set; }

    public int ExpireInSeconds { get; set; }
}