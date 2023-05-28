using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PaymentManagement.Application.Contracts.PaymentManagement.Dtos
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
