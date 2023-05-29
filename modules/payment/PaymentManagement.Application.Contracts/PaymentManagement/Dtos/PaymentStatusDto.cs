using System.Runtime.Serialization;

namespace PaymentManagement.Application.Contracts.Dtos
{
    [DataContract]
    public class PaymentStatusDto
    {
        [DataMember(Order = 1)]
        public int? RelationId { get; set; }
        [DataMember(Order = 2)]
        public int? RelationIdB { get; set; }
        [DataMember(Order = 3)]
        public int? RelationIdC { get; set; }
        [DataMember(Order = 4)]
        public int? RelationIdD { get; set; }

    }
}
