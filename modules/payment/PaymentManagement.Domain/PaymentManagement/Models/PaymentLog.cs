using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace PaymentManagement.Domain.Models
{
    [Table("PaymentLog", Schema = "dbo")]
    public class PaymentLog : FullAuditedEntity<int>
    {
        public int PaymentId { get; set; }
        [Required]
        [Column(TypeName = "NVARCHAR(100)")]
        public string Psp { get; set; }
        [Required]
        [Column(TypeName = "NVARCHAR(100)")]
        public string Message { get; set; }
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Parameter { get; set; }
        public virtual Payment Payment { get; set; }
    }
}
