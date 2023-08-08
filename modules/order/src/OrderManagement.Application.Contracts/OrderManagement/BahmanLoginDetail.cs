using Newtonsoft.Json;

namespace OrderManagement.Application.Contracts
{
    public class BahmanLoginDetail
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("tokenExpireTime")]
        public DateTime TokenExpireTime { get; set; }
        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
        [JsonProperty("refreshTokenExpireDate")]
        public DateTime RefreshTokenExpireDate { get; set; }
    }

}