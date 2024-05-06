using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;

namespace UserManagement.Domain.UserManagement.Bases
{
    [Table("City", Schema = "aucbase")]
    public class City:Entity<int>
    {
        [Column(TypeName = "NVARCHAR(100)")]
        public string Name { get; set; }

        public int ProvinceId { get; set; }
        public virtual Province Province { get; set; }
    }
}
