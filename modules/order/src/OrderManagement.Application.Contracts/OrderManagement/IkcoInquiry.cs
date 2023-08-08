using Newtonsoft.Json;

namespace OrderManagement.Application.Contracts
{
    public class IkcoInquiry
    {
        [JsonProperty("nationalCode")]
        public string NationalCode { get; set; }
        [JsonProperty("tranDate")]
        public string TranDate { get; set; }
        [JsonProperty("payedPrice")]
        public decimal PayedPrice { get; set; }
        [JsonProperty("contRowId")]
        public int ContRowId { get; set; }
        [JsonProperty("vin")]
        public string Vin { get; set; }
        [JsonProperty("bodyNumber")]
        public string BodyNumber { get; set; }
        [JsonProperty("deliveryDate")]
        public string DeliveryDate { get; set; }
        [JsonProperty("finalPrice")]
        public long FinalPrice { get; set; }
        [JsonProperty("CarDesc")]
        public string CarDesc { get; set; }
    }
}