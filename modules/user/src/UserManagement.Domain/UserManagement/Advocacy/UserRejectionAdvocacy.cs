using Abp.Domain.Entities.Auditing;
using Castle.MicroKernel.SubSystems.Conversion;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Domain.UserManagement.Advocacy
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
