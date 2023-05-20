using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace PaymentManagement.Domain.Models
{
    [Table("Account", Schema = "dbo")]
    public class Account : FullAuditedEntity<int>
    {
        public int CustomerId { get; set; }
        [Required]
        [Column(TypeName = "NVARCHAR(100)")]
        public string AccountName { get; set; }
        [Column(TypeName = "NVARCHAR(100)")]
        public string Branch { get; set; }
        [Column(TypeName = "NVARCHAR(100)")]
        public string AccountNumber { get; set; }
        [Column(TypeName = "NVARCHAR(100)")]
        public string IBAN { get; set; }
        public bool IsActive { get; set; }
        public virtual Customer Customer { get; set; }
        private ICollection<PspAccount> _pspAccounts;
        public virtual ICollection<PspAccount> PspAccounts
        {
            get => _pspAccounts ??= new List<PspAccount>();
            protected set => _pspAccounts = value;
        }
    }
}
