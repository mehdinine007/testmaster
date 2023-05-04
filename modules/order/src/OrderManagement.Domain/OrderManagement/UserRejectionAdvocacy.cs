using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class UserRejectionAdvocacy : FullAuditedEntity<int>
    {
        [Column(TypeName = "VARCHAR(10)")]
        [Required]
        public string NationalCode { get; set; }
        public bool Archived { get; set; }
        public int SaleId { get; set; }
        [Required]
        public DateTime datetime { get; set; }
        [Required]
        [Column(TypeName = "VARCHAR(26)")]
        public string ShabaNumber { get; set; }
        [Column(TypeName = "NVARCHAR(50)")]
        [Required]
        public string accountNumber { get; set; }
        public int BatchId { get; set; }


    }
}
