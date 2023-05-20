using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace PaymentManagement.Domain.Models
{
    [Table("PaymentStatus", Schema = "dbo")]
    public class PaymentStatus : FullAuditedEntity<int>
    {
        public string Title { get; set; }
        private ICollection<Payment> _payments;
        public virtual ICollection<Payment> Payments
        {
            get => _payments ??= new List<Payment>();
            protected set => _payments = value;
        }
    }
}
