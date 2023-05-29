using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace PaymentManagement.Domain.Models
{
    [Table("Payment", Schema = "dbo")]
    [Index(nameof(PaymentStatusId), nameof(TransactionDate), nameof(RetryCount))]
    [Index(nameof(PaymentStatusId), nameof(FilterParam1), nameof(FilterParam2), nameof(FilterParam3), nameof(FilterParam4))]
    public class Payment : FullAuditedEntity<int>
    {
        public int PspAccountId { get; set; }
        public int PaymentStatusId { get; set; }
        [Precision(10, 0)]
        public decimal Amount { get; set; }
        [Column(TypeName = "VARCHAR(100)")]
        public string? Token { get; set; }
        [Column(TypeName = "VARCHAR(100)")]
        public string? TransactionCode { get; set; }
        [Column(TypeName = "VARCHAR(50)")]
        public string? TraceNo { get; set; }
        public DateTime TransactionDate { get; set; }
        [Required]
        [Column(TypeName = "VARCHAR(10)")]
        public string TransactionPersianDate { get; set; }
        [Column(TypeName = "VARCHAR(10)")]
        public string? NationalCode { get; set; }
        [Column(TypeName = "VARCHAR(20)")]
        public string? Mobile { get; set; }
        [Required]
        [Column(TypeName = "VARCHAR(200)")]
        public string CallBackUrl { get; set; }
        public int? FilterParam1 { get; set; }
        public int? FilterParam2 { get; set; }
        public int? FilterParam3 { get; set; }
        public int? FilterParam4 { get; set; }
        public int RetryCount { get; set; }
        public virtual PspAccount PspAccount { get; set; }
        public virtual PaymentStatus PaymentStatus { get; set; }
        private ICollection<PaymentLog> _paymentLogs;
        public virtual ICollection<PaymentLog> PaymentLogs
        {
            get => _paymentLogs ??= new List<PaymentLog>();
            protected set => _paymentLogs = value;
        }
    }
}
