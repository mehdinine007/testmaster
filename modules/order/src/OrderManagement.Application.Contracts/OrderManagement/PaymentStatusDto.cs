using System.Runtime.Serialization;

namespace OrderManagement.Application.Contracts
{
    public class PaymentStatusDto
    {
        public int RelationId { get; set; }
        public int RelationIdB { get; set; }
        public int RelationIdC { get; set; }
        public int RelationIdD { get; set; }
        public bool IsRelationIdGroup  { get; set; }
        public bool IsRelationIdBGroup { get; set; }
        public bool IsRelationIdCGroup { get; set; }
        public bool IsRelationIdDGroup { get; set; }

    }
}
