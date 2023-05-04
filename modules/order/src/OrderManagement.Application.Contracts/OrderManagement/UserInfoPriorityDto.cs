using Newtonsoft.Json;

namespace OrderManagement.Application.Contracts
{
    public class UserInfoPriorityDto
    {
        public string NationalCode { get; set; }
        [JsonProperty(PropertyName = "شهرمحل تولد")]
        public string BirthCityName { get; set; }

        public string Mobile { get; set; }
    }
}