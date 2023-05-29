using System.Runtime.Serialization;

namespace PaymentManagement.Application.Contracts.Dtos
{
    [DataContract]
    public class InquiryWithFilterParamDto
    {
        [DataMember(Order = 1)]
        public int Status { get; set; }
        [DataMember(Order = 2)]
        public string Message { get; set; }
        [DataMember(Order = 3)]
        public int Count { get; set; }
    }
}