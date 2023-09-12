using Newtonsoft.Json;

namespace UserManagement.Application.Contracts.Models;

public class FaraBoomZipCodeInquiryResponse
{
    [JsonProperty(PropertyName = "operation_time")]
    public long OperationTime { get; set; }

    [JsonProperty(PropertyName = "ref_id")]
    public string RefId { get; set; }

    [JsonProperty(PropertyName = "address")]
    public string Addresss { get; set; }
}