using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace OrderManagement.Domain
{
    public class PreSale : FullAuditedEntity<int>
    {
        [Required]
        [Column(TypeName = "NVARCHAR(150)")]
        public string Brand { get; set; }
        [Required]
        [Column(TypeName = "NVARCHAR(150)")]
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
