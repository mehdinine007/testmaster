using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace UserManagement.Domain.UserManagement.Bases
{
    [Table("Province", Schema = "aucbase")]
    public class Province:Entity<int>
    {
        [Column(TypeName ="NVARCHAR(100)")]
        public string Name { get; set; } 
        public virtual ICollection<City> ProvinceCities{get;set;}
    }
}