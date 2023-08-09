using Newtonsoft.Json;

namespace OrderManagement.Application.Contracts
{
    public class BahmanLoginResult
    {
        [JsonProperty("item")]
        public BahmanLoginDetail BahmanLoginDetail { get; set; }
        [JsonProperty("responseCode")]
        public int ResponseCode { get; set; }
        [JsonProperty("msg")]
        public string Message { get; set; }
        [JsonProperty("title")]
        public object Title { get; set; }
    }
}