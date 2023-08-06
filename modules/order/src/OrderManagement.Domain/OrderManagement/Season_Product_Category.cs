using OrderManagement.Domain.OrderManagement;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class Season_Product_Category : FullAuditedEntity<int>
    {
        [ForeignKey("Season")]
        public int SeasonId { get; set; }
        public virtual Season Season { get; set; }
        public int Count { get; set; }
        [ForeignKey("ESaleType")]
        public int? EsaleTypeId { get; set; }
        public virtual ESaleType ESaleType { get; set; }
        public int? YearId { get; set; }
        public virtual Year Year { get; set; }
        public int? ProductId { get; set; }
        public virtual ProductAndCategory Product { get; set; }
        public int? CategoryId { get; set; }
        public virtual ProductAndCategory Category { get; set; }

        //[ForeignKey("CarTip")]
        public int CarTipId { get; set; }

        //public virtual CarTip CarTip { get; set; }

        public int CompanyId { get; set; }
    }
}
