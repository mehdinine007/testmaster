using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace PaymentManagement.Domain.Models
{
    [Table("Customer", Schema = "dbo")]
    public class Customer : FullAuditedEntity<int>
    {
        [Required]
        [Column(TypeName = "NVARCHAR(300)")]
        public string Title { get; set; }
        //todo: be unique
        public int Code { get; set; }
        public bool IsActive { get; set; }
        private ICollection<Account> _accounts;
        public virtual ICollection<Account> Accounts
        {
            get => _accounts ??= new List<Account>();
            protected set => _accounts = value;
        }
    }
}
