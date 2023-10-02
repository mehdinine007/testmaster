using Volo.Abp.Domain.Entities.Auditing;
using System;

namespace UserManagement.Domain.UserManagement.CompanyService
{
    public class ClientsOrderDetailByCompany : FullAuditedEntity<long>
    {
        public string NationalCode { get; set; }

        public int? SaleType { get; set; }

        public int? ModelType { get; set; }

        public DateTime? TurnDate { get; set; }

        public DateTime? InviteDate { get; set; }

        public DateTime? TranDate { get; set; }

        public long? PayedPrice { get; set; }

        public string ContRowId { get; set; }

        public DateTime? ContRowIdDate { get; set; }

        public string Vin { get; set; }

        public string BodyNumber { get; set; }

        public decimal? CooperateBenefit { get; set; }

        public decimal? CancelBenefit { get; set; }

        public decimal? DelayBenefit { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public long? FinalPrice { get; set; }

        public string CarDesc { get; set; }

        public long OrderId { get; set; }
    }
}
