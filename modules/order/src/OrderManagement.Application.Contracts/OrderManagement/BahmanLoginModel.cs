using Newtonsoft.Json;

namespace OrderManagement.Application.Contracts
{
    public class BahmanLoginModel
    {
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }

}