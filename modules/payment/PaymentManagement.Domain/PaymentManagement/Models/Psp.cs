using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace PaymentManagement.Domain.Models
{
    [Table("Psp", Schema = "dbo")]
    public class Psp : FullAuditedEntity<int>
    {
        public Psp()
        {
            PspAccounts = new List<PspAccount>();
        }

        [Required]
        [Column(TypeName = "NVARCHAR(100)")]
        public string Title { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<PspAccount> PspAccounts { get; set; }
    }
}
