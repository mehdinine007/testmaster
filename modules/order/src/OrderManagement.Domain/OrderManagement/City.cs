using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities;

namespace OrderManagement.Domain
{
    [Table("City", Schema = "aucbase")]
    public class City : Entity<int>
    {

        [Column(TypeName = "NVARCHAR(100)")]
        public string Name { get; set; }

        public int ProvinceId { get; set; }
        public virtual Province Province { get; set; }
    }
}
