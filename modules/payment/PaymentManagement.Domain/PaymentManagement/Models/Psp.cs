using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;

namespace PaymentManagement.Domain.Models
{
    [Table("Psp", Schema = "dbo")]
    [Index(nameof(IsActive))]
    public class Psp : FullAuditedEntity<int>
    {
        [Required]
        [Column(TypeName = "NVARCHAR(100)")]
        public string Title { get; set; }
        public bool IsActive { get; set; }

        private ICollection<PspAccount> _pspAccounts;
        public virtual ICollection<PspAccount> PspAccounts
        {
            get => _pspAccounts ??= new List<PspAccount>();
            protected set => _pspAccounts = value;
        }

    }
}
