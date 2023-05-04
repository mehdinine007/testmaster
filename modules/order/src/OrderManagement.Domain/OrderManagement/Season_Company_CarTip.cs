using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class Season_Company_CarTip : FullAuditedAggregateRoot<int>
    {
        public int CompanyId { get; set; }
        [ForeignKey("Season")]
        public int SeasonId { get; set; }
        public virtual Season Season { get; set; }
        [ForeignKey("CarTip")]
        public int CarTipId { get; set; }
        public virtual CarTip CarTip { get; set; }
        public int Count { get; set; }
        [ForeignKey("ESaleType")]
        public int? EsaleTypeId { get; set; }
        public virtual ESaleType ESaleType { get; set; }
        public int? YearId { get; set; }
        public virtual Year Year { get; set; }
    }

}
