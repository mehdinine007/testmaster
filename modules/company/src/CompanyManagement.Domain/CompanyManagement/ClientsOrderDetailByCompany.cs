using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;

namespace CompanyManagement.Domain.CompanyManagement;

public class ClientsOrderDetailByCompany : FullAuditedEntity<long>
{
    public string NationalCode { get; set; }
    public int? SaleType { get; set; }
    public int? ModelType { get; set; }
    public DateTime? InviteDate { get; set; }
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
    public string CarCode { get; set; }
    public string CompanySaleId { get; set; }

    public string TrackingCode { get; set; }
    public virtual ICollection<CompanyPaypaidPrices> Paypaidprice { get; set; }
    public virtual ICollection<CompanySaleCallDates> TurnDate { get; set; }
    public int DeliveryYear { get; set; }
    public int DeliveryMonth { get; set; }
    public int DeliveryDay { get; set; }
    public int IntroductionYear { get; set; }
    public int IntroductionMonth { get; set; }
    public int IntroductionDay { get; set; }
    public int FactorYear { get; set; }
    public int FactorMonth { get; set; }
    public int FactorDay { get; set; }
    public int CompanyId { get; set; }

    public bool RelatedToOrganization { get; set; }
}
