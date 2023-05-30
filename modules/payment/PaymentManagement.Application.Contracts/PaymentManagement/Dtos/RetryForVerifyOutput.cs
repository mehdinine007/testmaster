using System.Runtime.Serialization;

namespace PaymentManagement.Application.Contracts.Dtos
{
    public class RetryForVerifyOutput
    {
        [DataMember(Order = 1)]
        public int PaymentId { get; set; }
        [DataMember(Order = 2)]
        public int StatusId { get; set; }
    }

    [DataContract]
    public class RetryForVerifyDetail : RetryForVerifyOutput
    {
        [DataMember(Order = 3)]
        public int? FilterParam1 { get; set; }
        [DataMember(Order = 4)]
        public int? FilterParam2 { get; set; }
        [DataMember(Order = 5)]
        public int? FilterParam3 { get; set; }
        [DataMember(Order = 6)]
        public int? FilterParam4 { get; set; }
    }

}
