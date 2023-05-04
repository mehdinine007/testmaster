using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class UserRejectionFromBank : FullAuditedEntity<int>
    {
        [Required]
        public string nationalcode { get; set; }
        public string bankName { get; set; }
        public decimal price { get; set; }
        public DateTime? dateTime { get; set; }
        public string accountNumber { get; set; }
        public string shabaNumber { get; set; }
        public long UserId { get; set; }
        public int? BanksId { get; set; }
        public virtual Bank Banks { get; set; }
        public string CarMaker { get; set; }
    }

}
