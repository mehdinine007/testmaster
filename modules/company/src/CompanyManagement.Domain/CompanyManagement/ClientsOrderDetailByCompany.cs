using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;

namespace CompanyManagement.Domain.CompanyManagement
{
    public class ClientsOrderDetailByCompany : FullAuditedEntity<long>
    {
        public string NationalCode { get; set; }
        public int? SaleType { get; set; }
        public int? ModelType { get; set; }
        public DateTime? InviteDate { get; set; }
        public DateTime? TranDate { get; set; }
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
        public bool? IsCanceled { get; set; }
        public DateTime? IntroductionDate { get; set; }
        public DateTime? FactorDate { get; set; }
        public virtual ICollection<CompanyPaypaidPrices> Paypaidprice { get; set; }
        public virtual ICollection<CompanySaleCallDates> TurnDate { get; set; }
    }
}
