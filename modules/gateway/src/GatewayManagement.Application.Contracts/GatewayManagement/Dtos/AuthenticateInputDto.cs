namespace GatewayManagement.Application.Contracts.Dtos
{
    public class AuthenticateInputDto
    {
        public string Type { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
