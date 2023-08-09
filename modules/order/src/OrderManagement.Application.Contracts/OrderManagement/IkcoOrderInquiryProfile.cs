using Newtonsoft.Json;

namespace OrderManagement.Application.Contracts
{
    public class IkcoOrderInquiryProfile
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("userPWD")]
        public string UserPWD { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }
    }
}