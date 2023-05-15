using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;
using System;

namespace OrderManagement.Domain
{
    public class PreSale : FullAuditedEntity<int>
    {
        [Required]
        [Column(TypeName = "string(150)")]
        public string Brand { get; set; }
        [Required]
        [Column(TypeName = "string(150)")]
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
