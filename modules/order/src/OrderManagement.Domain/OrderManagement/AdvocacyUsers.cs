using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class AdvocacyUser : FullAuditedEntity<int>
    {
        [Required]
        [Column(TypeName = "NCHAR(10)")]
        public string nationalcode { get; set; }
        public string bankName { get; set; }
        public decimal price { get; set; }
        public DateTime? dateTime { get; set; }
        public string accountNumber { get; set; }
        public string shabaNumber { get; set; }
        public long UserId { get; set; }
        public int? BanksId { get; set; }
    }
}
