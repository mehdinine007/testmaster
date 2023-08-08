using Newtonsoft.Json;

namespace OrderManagement.Application.Contracts
{
    public class IkcoLogin
    {
        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }
        [JsonProperty("refreshToken")]
        public object RefreshToken { get; set; }
    }
}