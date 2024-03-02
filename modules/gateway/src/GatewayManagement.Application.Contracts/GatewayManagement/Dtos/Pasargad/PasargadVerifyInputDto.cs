namespace GatewayManagement.Application.Contracts.Dtos
{
    public class PasargadVerifyInputDto
    {
        public string Token { get; set; }
        public string Invoice { get; set; }
        public string UrlId { get; set; }
    }
}
