using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Domain.UserManagement.Sale
{

    public class PreSale: FullAuditedEntity<int>
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
