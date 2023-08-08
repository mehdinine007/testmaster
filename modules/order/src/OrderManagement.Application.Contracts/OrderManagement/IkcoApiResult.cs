using Newtonsoft.Json;

namespace OrderManagement.Application.Contracts
{
    public class IkcoApiResult<T>
    {
        [JsonProperty("succeeded")]
        public bool Succeeded { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("errors")]
        public string Errors { get; set; }
        public T Data { get; set; }
        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }
    }
}