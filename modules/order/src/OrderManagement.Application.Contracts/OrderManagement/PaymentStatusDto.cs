using System.Runtime.Serialization;

namespace OrderManagement.Application.Contracts
{
    public class PaymentStatusDto
    {
        public int RelationId { get; set; }
        public int RelationIdB { get; set; }
        public int RelationIdC { get; set; }
        public int RelationIdD { get; set; }

    }
}
